/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    public class Scannable : Interactable
    {
        [SerializeField] private int libaryIndex;
        [SerializeField, ColorUsage(true, true)] private Color scanLockedColor = Color.blue;
        [SerializeField, ColorUsage(true, true)] private Color scanUnlockedColor = Color.red;
        
        public int LibaryIndex => libaryIndex;   
        public Color ScanLockedColor => scanLockedColor;
        public Color ScanUnlockedColor => scanUnlockedColor;
    }
}





