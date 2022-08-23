/// <author>Thoams Krahl</author>

using System;
using System.Collections.Generic;
using UnityEngine;
using PelagosProject.User.Input;
using PelagosProject.Interactables;
using PelagosProject.Puzzles;
using PelagosProject.UI;
using PelagosProject.Audio;

namespace PelagosProject
{
    public class Scanner : MonoBehaviour
    {
        #region Events

        public static Action<bool, int> ScannableIsOnFocus;
        public static Action<bool> PuzzleIsOnFocus;

        public static Action<bool,float, float> UpdateScanProgress;
        public static Action ScanIsComplete;

        #endregion

        #region SerializedFields

        [SerializeField, Range(0f, 999f)] private float maxScanRange = 100f;
        [SerializeField, Range(0.01f, 5.0f)] private float scannableScanDuration = 1f;
        [SerializeField, Range(0.01f, 5.0f)] private float puzzleScanDuration = 1f;
        [SerializeField] private AudioEventList shipAudioEvents;

        #endregion

        #region PrivateFields

        private PlayerInput input;
        private bool puzzleMode = false;
        private float currentScanTime;
        private Interactable interactableOnFocus;
        private GameObject terrainTreePrefab;
        private GameObject terrainTreeGhost;
        private List<MeshRenderer> interactebleOnFocusMeshRenderes = new List<MeshRenderer>();
        private bool onScan;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Puzzle.PuzzleStarted += SetPuzzleScanMode;
            Puzzle.PuzzleComplete += SetNormalScanMode;
        }

        private void Start()
        {
            input = Game.Instance.Input;
        }

        private void OnDestroy()
        {
            Puzzle.PuzzleStarted -= SetPuzzleScanMode;
            Puzzle.PuzzleComplete -= SetNormalScanMode;
        }


        private void Update()
        {
            UpdateScan();
        }

        #endregion

        #region ScanMode

        public void SetNormalScanMode()
        {
            puzzleMode = false;
        }

        public void SetPuzzleScanMode(Sprite sprite)
        {
            puzzleMode = true;
        }

        #endregion

        #region Scan

        private void UpdateScan()
        {
            if (!input.ScanInputIsPressed)
            {
                if(!onScan) return;
                onScan = false;
                ScannableIsOnFocus?.Invoke(false, -1);
                ResetScanTime();
                shipAudioEvents.StopEvent("Scan_Hint");
                return;
            }
            onScan = true;
            shipAudioEvents.CreateEvent("Scan_Hint");
            Scan();           
        }

        private void Scan()
        {
            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = Camera.main.transform.forward;

            Ray ray = new Ray(rayOrigin, rayDirection);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * maxScanRange, Color.red);
          
            if (Physics.Raycast(ray, out hit, maxScanRange))
            {
                //Debug.Log(hit.collider.name + " / " + hit.collider.gameObject.layer);
                var interactable = hit.collider.GetComponent<Interactable>();

                if (interactable == null)
                {
                    if(puzzleMode) return;
                    ScannableIsOnFocus?.Invoke(false, -1);
                    if(currentScanTime > 0f) ResetScanTime();
                    return;
                }
                interactableOnFocus = interactable;
                CheckInteractable(hit);

                float distance = Vector3.Distance(transform.position, hit.point);
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);      
                
            }
            else
            {
                ScannableIsOnFocus?.Invoke(false, -1);
                ResetScanTime();
            }
        }

        private void ScanComplete()
        {
            ScanIsComplete.Invoke();

            switch (interactableOnFocus.Type)
            {
                case InteractableBaseType.Invalid:
                    Debug.LogError("ERROR - Invalid InteractableType");
                    break;

                case InteractableBaseType.Terrain:

                    if (terrainTreePrefab == null) return;

                    var terrainScannable = terrainTreePrefab.GetComponent<Scannable>();
                    if (terrainScannable == null)
                    {
                        Debug.LogError("NO SCANNABLE FOUND");
                        return;
                    }

                    ScannableLibary.Instance.UnlockCreature(terrainScannable.LibaryIndex);
                    ScannableIsOnFocus?.Invoke(true, terrainScannable.LibaryIndex);
                    break;


                case InteractableBaseType.Object:
                    if (puzzleMode) return;
                    break;


                case InteractableBaseType.Scannable:

                    if (puzzleMode) return;

                    var scannable = interactableOnFocus as Scannable;
                    if (scannable == null)
                    {
                        Debug.LogError("NO SCANNABLE FOUND");
                        return;
                    }

                    ScannableLibary.Instance.UnlockCreature(scannable.LibaryIndex);
                    ScannableIsOnFocus?.Invoke(true, scannable.LibaryIndex);
                    //SetMaterialProperties(0, )
                    break;

                case InteractableBaseType.PuzzleObject:
                    var puzzleObject = interactableOnFocus as PuzzleInteractable;
                    float angle = Vector3.Angle(transform.position, interactableOnFocus.transform.position);
                    puzzleObject.ElementScanned(angle);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Interactable

        private void CheckInteractable(RaycastHit raycastHit)
        {
            if (interactebleOnFocusMeshRenderes.Count != 0) SetMaterialProperties(0f, false, interactableOnFocus as Scannable);

            switch (interactableOnFocus.Type)
            {
                case InteractableBaseType.Invalid:
                    Debug.LogError("ERROR - Invalid InteractableType");
                    break;


                case InteractableBaseType.Terrain:

                    // -> Deprecated <-

                    //var terrain = interactableOnFocus.GetComponent<Terrain>();
                    //CheckTerrainForScannable(terrain, raycastHit);
                    break;



                case InteractableBaseType.Object:
                    if (puzzleMode) return;
                    break;


                case InteractableBaseType.Scannable:

                    if (puzzleMode) return;
                    
                    var scannable = interactableOnFocus as Scannable;
                    if (scannable == null)
                    {
                        Debug.LogError("NO SCANNABLE FOUND");
                        ResetScanTime();
                        return;
                    }
                 
                    bool unlocked = ScannableLibary.Instance.CheckUnlockState(scannable.LibaryIndex);

                    ClearMeshRendererList();
                    GetMeshRenderers(interactableOnFocus.gameObject);
                    SetMaterialProperties(1f, unlocked, scannable);

                    ScannableIsOnFocus?.Invoke(true, scannable.LibaryIndex);
                    if (unlocked) return;
                    UpdateScanTime(scannableScanDuration);                  
                    break;

                case InteractableBaseType.PuzzleObject:
                    var puzzleObject = interactableOnFocus as PuzzleInteractable;
                    if (puzzleObject.isScanned) return;
                    UpdateScanTime(puzzleScanDuration);        
                    break;

                default:
                    ResetScanTime();
                    break;
            }
        }

        private void GetMeshRenderers(GameObject gameObject)
        {
            int count = gameObject.transform.childCount;

            gameObject.TryGetComponent(out MeshRenderer mr);
            if (mr != null) interactebleOnFocusMeshRenderes.Add(mr);

            if(count == 0) return;

            for (int i = 0; i < count; i++)
            {
                gameObject.transform.GetChild(i).TryGetComponent(out mr);
                if (mr != null) interactebleOnFocusMeshRenderes.Add(mr);
            }
        }

        private void ClearMeshRendererList()
        {
            interactebleOnFocusMeshRenderes.Clear();
        }

        private void SetMaterialProperties(float value, bool unlocked, Scannable scannable)
        {
            Color colorLocked = new Vector4(0f,0f,0f,0f);
            Color colorUnlocked = new Vector4(0f,0f,0f,0f);

            if (scannable != null) colorLocked = scannable.ScanLockedColor;
            if (scannable != null) colorUnlocked = scannable.ScanUnlockedColor;

            if (interactebleOnFocusMeshRenderes.Count == 0) return;
            
            foreach (var meshRenderer in interactebleOnFocusMeshRenderes)
            {
                var material = meshRenderer.materials[0];

                if (material == null) continue;
                material.SetFloat("_ScanIntensity", value);

                if (unlocked)
                {
                    material.SetFloat("_UseTheScrollTexture", 0f);

                    if (scannable == null) continue;
                    material.SetColor("_FresnelColor", colorUnlocked);
                    material.SetColor("_ScrollTextureColor", colorUnlocked);
                    
                }
                else
                {
                    material.SetFloat("_UseTheScrollTexture", 1f);

                    if (scannable == null) continue;
                    material.SetColor("_FresnelColor", colorLocked);
                    material.SetColor("_ScrollTextureColor", colorLocked);
                }
            }                                    
        }

        // -> Deprecated <-
        /*private void CheckTerrainForScannable(Terrain terrain, RaycastHit raycastHit)
        {
            terrainTreePrefab = null;
            

            test1.transform.position = raycastHit.point;

            //Vector3 playerPosition = Game.Instance.Player.transform.position;
            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
            Vector3 terrainSize = terrain.terrainData.size;
            Vector3 terrainposition = terrain.transform.position;

            Vector3 treeWorldPostion = Vector3.zero;
            Vector3 treePosition = Vector3.zero;
            TreeInstance treeInstance;
            int nearestTreeIndex = 0;
            float distance;
            float nearestDistance = 5f;

            //Find the nearest Tree on Terrain;
            for (int i = 0; i < treeInstances.Length; i++)
            {
                //tree instance
                treeInstance = treeInstances[i];
                //tree instance position;
                treePosition = treeInstance.position;
               
                //world coordinates of the tree instance position
                treeWorldPostion = Vector3.Scale(terrainSize, treePosition) + terrainposition;
                //distance
                distance = Vector3.Distance(treeWorldPostion, raycastHit.point);


                if (distance > nearestDistance) continue;
                
                nearestDistance = distance;
                nearestTreeIndex = i;
                //GetPrototypIndex
                int prototypeIndex = treeInstance.prototypeIndex;
         
                //SetTestObject
                terrainTreePrefab = terrain.terrainData.treePrototypes[prototypeIndex].prefab;
                test2.transform.position = treeWorldPostion;

            }

            if (terrainTreePrefab == null) return;
           
            if(terrainTreeGhost != null) Destroy(terrainTreeGhost);


            treeInstance = treeInstances[nearestTreeIndex];
            treePosition = treeInstance.position;
            treeWorldPostion = Vector3.Scale(terrainSize, treePosition) + terrainposition;
            float height = treeInstance.heightScale;
            float width = treeInstance.widthScale;
            float rotation = treeInstance.rotation;

            terrainTreeGhost = Instantiate(terrainTreePrefab, treeWorldPostion, Quaternion.Euler(0f, terrainTreePrefab.transform.rotation.y + rotation, 0f));
            terrainTreeGhost.transform.localScale = new Vector3(terrainTreeGhost.transform.localScale.x * width, terrainTreeGhost.transform.localScale.y * height, terrainTreeGhost.transform.localScale.z * width);

            Debug.Log("Found a Scannable Tree on Terrain");
            var scannable = terrainTreePrefab.GetComponent<Scannable>();

            bool unlocked = ScannableLibary.Instance.CheckUnlockState(scannable.LibaryIndex);
         
            //terrain.drawTreesAndFoliage = false;
            ClearMRList();
            GetMeshRenderers(terrainTreeGhost);
            SetMaterialProperties(1f);

            ScannableIsOnFocus?.Invoke(true, scannable.LibaryIndex);
            if (!unlocked)
            {
                UpdateScanTime(scannableScanDuration);
            }

        }*/

        #endregion

        #region ScanTime

        private void UpdateScanTime(float timeToUnlock)
        {
            currentScanTime += Time.deltaTime;

            if (currentScanTime >= timeToUnlock)
            {
                ScanComplete();
                ResetScanTime();
                return;
            }
            UpdateScanProgress?.Invoke(true, currentScanTime, timeToUnlock);

        }

        private void ResetScanTime()
        {
            currentScanTime = 0f;
            UpdateScanProgress?.Invoke(false, currentScanTime, 0f);

            SetMaterialProperties(0f, false, interactableOnFocus as Scannable);
            ClearMeshRendererList();

            interactableOnFocus = null;
            if (terrainTreeGhost != null) Destroy(terrainTreeGhost, 0.1f);
        }

        #endregion
    }
}


