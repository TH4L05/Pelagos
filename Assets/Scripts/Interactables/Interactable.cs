/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractableBaseType interactableType = InteractableBaseType.Invalid;
        [SerializeField] private bool isInteractable = true;

        public InteractableBaseType Type => interactableType;
    }

}

