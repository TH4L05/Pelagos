/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace PelagosProject.Audio
{
    public class Trigger : MonoBehaviour
    {
        #region Fields

        [Space(5)]
        [Header("Parameters")]
        [SerializeField] private bool destroyOnEnter = false;
        [SerializeField] private bool destroyOnExit = false;
        [SerializeField] private bool playerOnly = false;

        [Space(5)]
        [Header("Events")]
        public UnityEvent OnObjectTriggerEnter;
        public UnityEvent OnObjectTriggerStay;
        public UnityEvent OnObjectTriggerExit;

        [Space(5)]
        [Header("Dev")]
        [SerializeField] private Color gizmoColor = Color.cyan;

        [HideInInspector] public GameObject objInZone;

        #endregion

        #region Unity Functions

        private void OnTriggerEnter(Collider collider)
        {
            //Debug.Log(collider.name);
            objInZone = collider.gameObject;

            if (playerOnly)
            {
                if (collider.CompareTag("Player"))
                {
                    OnObjectTriggerEnter?.Invoke();

                    if (!destroyOnEnter) return;
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    return;
                }
            }
            OnObjectTriggerEnter?.Invoke();

            if (!destroyOnEnter) return;
            Destroy(gameObject);

        }


        public void OnTriggerStay(Collider collider)
        {
            if (playerOnly)
            {
                if (collider.CompareTag("Player"))
                {
                    OnObjectTriggerStay?.Invoke();
                    return;
                }
                else
                {
                    return;
                }
            }
            OnObjectTriggerStay?.Invoke();
        }

        private void OnTriggerExit(Collider collider)
        {
            if (playerOnly)
            {
                if (collider.CompareTag("Player"))
                {
                    OnObjectTriggerExit?.Invoke();

                    if (!destroyOnExit) return;
                    Destroy(gameObject);

                    return;
                }
                else
                {
                    return;
                }
            }
            OnObjectTriggerExit?.Invoke();

            if (!destroyOnExit) return;
            Destroy(gameObject);
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = gizmoColor;

            Collider coll = GetComponent<Collider>();
            switch (coll)
            {
                case BoxCollider:
                    var boxColl = coll as BoxCollider;

                    //Gizmos.DrawCube(transform.position, boxColl.bounds.size);
                    Gizmos.DrawCube(Vector3.zero, boxColl.size);
                    break;

                case SphereCollider:
                    var sphereColl = coll as SphereCollider;
                    Gizmos.DrawSphere(Vector3.zero, sphereColl.radius);
                    break;

                default:
                    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                    break;
            }
        }
    }
}

