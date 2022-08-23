// Copyright (c) 2016 Unity Technologies. MIT license - license_unity.txt
// #NVJOB Simple Boids. MIT license - license_nvjob.txt
// #NVJOB Nicholas Veselov - https://nvjob.github.io
// #NVJOB Simple Boids v1.1.1 - https://nvjob.github.io/unity/nvjob-boids
// Edits for Pelagos Project -> Thomas Krahl

using System.Collections;
using UnityEngine;

[HelpURL("https://nvjob.github.io/unity/nvjob-boids")]
//[AddComponentMenu("#NVJOB/Boids/Simple Boids")]

public class NVBoids : MonoBehaviour
{
    #region SerializedFields

    [Header("Settings")]
    [SerializeField] private BoidData boidData;
    [SerializeField] private bool debug;
    [SerializeField] private bool danger;
    [SerializeField] private Color gizmoColor = Color.red;
    [SerializeField] private float shaderSpeedMultiply = 1f;

    [Header("Information")] // These variables are only information.
    [SerializeField] private string HelpURL = "nvjob.github.io/unity/nvjob-boids";
    [SerializeField] private string ReportAProblem = "nvjob.github.io/support";
    [SerializeField] private string Patrons = "nvjob.github.io/patrons";


    #endregion

    #region PrivateFields

    private int dangerBird;
    private LayerMask dangerLayer;

    private Transform thisTransform;
    private Transform dangerTransform;
    private Transform[] birdsTransform;
    private Transform[] flocksTransform;

    private Vector3[] randomTargetPostions;
    private Vector3[] flockPos;
    private Vector3[] velFlocks;

    private float[] birdsSpeed;
    private float[] birdsSpeedCur;
    private float[] spVelocity;

    private int[] curentFlock;
    private float dangerSpeedCh;
    private float dangerSoaringCh;

    #endregion

    #region StaticFields

    private static WaitForSeconds delay0;
    private int birdsNum;
    private int flockNum;
    private int fragmentedFlock;

    #endregion

    #region UnityFunctions

    void Awake()
    {
        //--------------

        if (boidData == null)
        {
            Debug.LogError("Can not Create Boid -> Boid Data is missing");
            return;
        }

        /*if (boidData.birdPref.Length < 1)
        {
            Debug.LogError("Can not Create Boid -> Prefab is not Set");
            return;
        }*/

        birdsNum = boidData.birdsNum;
        flockNum = boidData.flockNum;
        fragmentedFlock = boidData.fragmentedFlock;

        thisTransform = transform;
        CreateFlock();
        CreateBird();
        StartCoroutine(BehavioralChange());

        dangerLayer = boidData.dangerLayer;
        StartCoroutine(Danger());

        //--------------
    }

    void LateUpdate()
    {
        //--------------  
        if (boidData == null) return;
        
        FlocksMove();
        BirdsMove();

        //--------------
    }

    #endregion

    #region Move

    void FlocksMove()
    {
        //--------------  

        for (int f = 0; f < flockNum; f++)
        {
            flocksTransform[f].localPosition = Vector3.SmoothDamp(flocksTransform[f].localPosition, flockPos[f], ref velFlocks[f], boidData.smoothChFrequency);
        }

        //--------------
    }

    void BirdsMove()
    {
        //--------------

        Vector3 translateCur = Vector3.forward * boidData.birdSpeed * dangerSpeedCh * Time.deltaTime;
        Vector3 verticalWaweCur = Vector3.up * ((boidData.verticalWawe * 0.5f) - Mathf.PingPong(Time.deltaTime * 0.5f, boidData.verticalWawe));
        float soaringCur = boidData.soaring * boidData.dangerSoaring * Time.deltaTime;

        //--------------

        for (int b = 0; b < birdsNum; b++)
        {
            if (birdsSpeedCur[b] != birdsSpeed[b]) birdsSpeedCur[b] = Mathf.SmoothDamp(birdsSpeedCur[b], birdsSpeed[b], ref spVelocity[b], 0.5f);
            birdsTransform[b].Translate(translateCur * birdsSpeed[b]);
            Vector3 tpCh = flocksTransform[curentFlock[b]].position + randomTargetPostions[b] + verticalWaweCur - birdsTransform[b].position;
            Quaternion rotationCur = Quaternion.LookRotation(Vector3.RotateTowards(birdsTransform[b].forward, tpCh, soaringCur, 0));
            if (boidData.rotationClamp == false) birdsTransform[b].rotation = rotationCur;
            else birdsTransform[b].localRotation = BirdsRotationClamp(rotationCur, boidData.rotationClampValue);

            //MeshRenderer mr = birdsTransform[b].GetComponent<MeshRenderer>();
            //Material material = mr.material;

            //float speed = birdsSpeed[b] * shaderSpeedMultiply;
            //material.SetFloat("_Speed", speed);
        }

        //--------------
    }

    static Quaternion BirdsRotationClamp(Quaternion rotationCur, float rotationClampValue)
    {
        //--------------

        Vector3 angleClamp = rotationCur.eulerAngles;
        rotationCur.eulerAngles = new Vector3(Mathf.Clamp((angleClamp.x > 180) ? angleClamp.x - 360 : angleClamp.x, -rotationClampValue, rotationClampValue), angleClamp.y, 0);
        return rotationCur;

        //--------------
    }
    
    IEnumerator BehavioralChange()
    {
        //--------------

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(boidData.behavioralCh.x, boidData.behavioralCh.y));

            //---- Flocks

            for (int f = 0; f < flockNum; f++)
            {
                if (Random.value < boidData.posChangeFrequency)
                {
                    Vector3 rdvf = Random.insideUnitSphere * fragmentedFlock;
                    flockPos[f] = new Vector3(rdvf.x, Mathf.Abs(rdvf.y * boidData.fragmentedFlockYLimit), rdvf.z);
                }
            }

            //---- Birds

            for (int b = 0; b < birdsNum; b++)
            {
                birdsSpeed[b] = Random.Range(3.0f, 7.0f);
                Vector3 lpv = Random.insideUnitSphere * boidData.fragmentedBirds;
                randomTargetPostions[b] = new Vector3(lpv.x, lpv.y * boidData.fragmentedBirdsYLimit, lpv.z);
                if (Random.value < boidData.migrationFrequency) curentFlock[b] = Random.Range(0, flockNum);
            } 
        }

        //--------------
    }

    #endregion

    #region Creation

    void CreateFlock()
    {
        //--------------

        flocksTransform = new Transform[flockNum];
        flockPos = new Vector3[flockNum];
        velFlocks = new Vector3[flockNum];
        curentFlock = new int[birdsNum];

        for (int f = 0; f < flockNum; f++)
        {
            GameObject nobj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nobj.SetActive(debug);
            flocksTransform[f] = nobj.transform;
            Vector3 rdvf = Random.onUnitSphere * fragmentedFlock;
            flocksTransform[f].position = thisTransform.position;
            flockPos[f] = new Vector3(rdvf.x, Mathf.Abs(rdvf.y * boidData.fragmentedFlockYLimit), rdvf.z);
            flocksTransform[f].parent = thisTransform;
        }

        //-------------- // For Danger and for flock hunter

        if (danger == true)
        {
            GameObject dobj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dobj.GetComponent<MeshRenderer>().enabled = debug;
            dobj.layer = gameObject.layer;
            dangerTransform = dobj.transform;
            dangerTransform.parent = thisTransform;
        }

        //--------------
    }

    void CreateBird()
    {
        //--------------

        birdsTransform = new Transform[birdsNum];
        birdsSpeed = new float[birdsNum];
        birdsSpeedCur = new float[birdsNum];
        randomTargetPostions = new Vector3[birdsNum];
        spVelocity = new float[birdsNum];

        for (int b = 0; b < birdsNum; b++)
        {
            //int prefabIndex = UnityEngine.Random.Range(0, boidData.birdPref.Length - 1);
            birdsTransform[b] = Instantiate(boidData.birdPref, thisTransform).transform;
            Vector3 lpv = Random.insideUnitSphere * boidData.fragmentedBirds;
            birdsTransform[b].localPosition = randomTargetPostions[b] = new Vector3(lpv.x, lpv.y * boidData.fragmentedBirdsYLimit, lpv.z);
            //birdsTransform[b].localScale = Vector3.one * Random.Range(scaleRandom.x, scaleRandom.y);
            birdsTransform[b].localRotation = Quaternion.Euler(0, Random.value * 360, 0);
            curentFlock[b] = Random.Range(0, flockNum);
            birdsSpeed[b] = Random.Range(3.0f, 7.0f);
        }

        //--------------
    }

    #endregion

    IEnumerator Danger()
    {
        //--------------

        if (danger == true)
        {
            delay0 = new WaitForSeconds(0.5f);

            while (true)
            {
                if (Random.value > 0.9f) dangerBird = Random.Range(0, birdsNum);
                dangerTransform.localPosition = birdsTransform[dangerBird].localPosition;

                if (Physics.CheckSphere(dangerTransform.position, boidData.dangerRadius, dangerLayer))
                {
                    dangerSpeedCh = boidData.dangerSpeed;
                    dangerSoaringCh = boidData.dangerSoaring;
                    yield return delay0;
                }
                else dangerSpeedCh = dangerSoaringCh = 1;

                yield return delay0;
            }
        }
        else dangerSpeedCh = dangerSoaringCh = 1;

        //--------------
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        if (boidData == null)
        {
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, boidData.fragmentedFlock);
        }
        
    }


}