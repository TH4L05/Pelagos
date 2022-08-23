/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables.Specials
{
    public class Geysir : MonoBehaviour
    {
        [SerializeField] private float fieldDistance = 40f;
        [SerializeField] private float forceMultiplier = 2f;
        [SerializeField] private Transform baseTransform;
        private GameObject objectInField;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                objectInField = collider.gameObject;
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                objectInField = collider.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            objectInField = null;
        }

        private void FixedUpdate()
        {
            AddForceToObjects();
        }

        private void AddForceToObjects()
        {
            if (objectInField == null) return;

            var rb = objectInField.GetComponent<Rigidbody>();        
            if (rb == null) return;       
            float distance = Vector3.Distance(baseTransform.position, objectInField.transform.position);
            float force = fieldDistance - distance;

            if (force > fieldDistance)
            {
                force = fieldDistance;
            }

            if (force < 0)
            {
                force = 0;
            }

            Debug.Log(force);

            Vector3 direction = baseTransform.position - objectInField.transform.position;
            //rb.velocity = Vector3.zero;
            rb.AddForce(-direction * forceMultiplier);

            //objectInField.transform.position += -direction * force * Time.fixedDeltaTime;
        }
    }
}

