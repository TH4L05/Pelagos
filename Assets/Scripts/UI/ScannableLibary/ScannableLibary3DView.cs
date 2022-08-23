/// <author>Thoams Krahl</author>

using UnityEngine;
using PelagosProject.Interactables;
using PelagosProject.UI.Menu.Ingame;

namespace PelagosProject.UI
{
    public class ScannableLibary3DView : MonoBehaviour
    {
        private enum RotationAxis
        {
            X,
            Y,
            Z
        }

        #region SerializedFields

        [SerializeField] private Camera cam;
        [SerializeField] private GameObject defaultDisplayedObject;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool isActive;
        [SerializeField] private RotationAxis rotationAxis = RotationAxis.Y;
        [SerializeField, Range(0.1f, 25.0f)] private float rotationSpeed = 2f;

        #endregion
        
        #region PrivateFields

        private GameObject displayedObject;
        private int divider = 1000;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            ScannableLibaryUISlot.ShowInformations += Display;          
        }

        private void Update()
        {
            Rotate();
        }

        #endregion

        public void SetStatus(bool active)
        {
            isActive = active;
        }

        private void Display(ScannableData data)
        {
            if(data == null) return;
        
            if (data.IsUnlocked)
            {
                GameObject prefab = data.Prefab;
                SpawnObject(prefab);
            }
            else
            {
                SpawnObject(null);
            }
        }

        private void Rotate()
        {
            if (displayedObject == null) return;

            switch (rotationAxis)
            {

                case RotationAxis.X:
                    displayedObject.transform.Rotate(displayedObject.transform.right, (rotationSpeed / divider) * Time.unscaledTime);
                    break;
                case RotationAxis.Y:
                    displayedObject.transform.Rotate(displayedObject.transform.up, (rotationSpeed / divider) * Time.unscaledTime);
                    break;
                case RotationAxis.Z:
                    displayedObject.transform.Rotate(displayedObject.transform.forward, (rotationSpeed / divider) * Time.unscaledTime);
                    break;

                default:
                    break;
            }


        }

        private void SpawnObject(GameObject gameObject)
        {
            if (displayedObject != null)
            {
                Debug.Log("Destroy");
                Destroy(displayedObject);
            }

            if (gameObject == null)
            {
                displayedObject = Instantiate(defaultDisplayedObject, spawnPoint.position, Quaternion.identity);
                displayedObject.transform.parent = transform;
                return;
            }

            displayedObject = Instantiate(gameObject, spawnPoint.position, Quaternion.identity);
            displayedObject.transform.parent = transform;
        }
    }
}

