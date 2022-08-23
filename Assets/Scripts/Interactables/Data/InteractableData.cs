/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    public enum InteractableBaseType
    {
        Invalid = -1,
        Object,
        Scannable,
        PuzzleObject,
        Terrain,
    }

    public class InteractableData : ScriptableObject
    {
        [SerializeField] protected new string name;
        [SerializeField, Multiline(3)] protected string description = "A description could go here";
       
        public string InteractableName => name;
        public string Description => description;
    }
}

