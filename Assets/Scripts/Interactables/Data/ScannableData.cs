/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    public enum ScannableType
    {
        Invalid = -1,
        Creature,
        Plant,
        Fungus
    }

    public class ScannableData : InteractableData
    {
        [Header("Base")]
        [SerializeField] protected bool unlocked = false;
        [SerializeField] protected ScannableType scannableType = ScannableType.Invalid;
        [SerializeField] protected Sprite unlockedSprite;
        [SerializeField] protected GameObject prefab;
   
        public bool IsUnlocked => unlocked;
        public ScannableType ScannableType => scannableType;
        public Sprite UnlockedSprite => unlockedSprite;
        public GameObject Prefab => prefab;

        public void Unlock()
        {
            unlocked = true;
        }

        public void Lock()
        {
            unlocked = false;
        }
    }
}

