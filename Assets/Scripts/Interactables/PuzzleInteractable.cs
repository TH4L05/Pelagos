/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    public enum PuzzleInteractableType
    {
        Invalid = -1,
        PuzzleStartObject,
        PuzzleReciveObject,
    }

    public class PuzzleInteractable : Interactable
    {
        [SerializeField] private PuzzleInteractableType puzzleInteractabletype;
        [SerializeField] private Sprite puzzleSprite;
        [SerializeField] private Vector3 puzzleAngles;

        public bool isScannable { get; set; }
        public bool isScanned = false;
        public Sprite PuzzleSprite => puzzleSprite;
        public PuzzleInteractableType PuzzleInteractableType => puzzleInteractabletype;


        private void Awake()
        {
            if(puzzleInteractabletype == PuzzleInteractableType.PuzzleStartObject) isScannable = true;
        }

        public void ElementScanned(float angle)
        {
            if (isScanned) return;
            if (!isScannable) return;

            switch (puzzleInteractabletype)
            {
                case PuzzleInteractableType.Invalid:

                    break;

                case PuzzleInteractableType.PuzzleStartObject:
                    isScanned = true;
                    break;


                case PuzzleInteractableType.PuzzleReciveObject:

                    if (puzzleAngles == Vector3.zero)
                    {
                        isScanned = true;
                        return;                     
                    }

                    if (angle < puzzleAngles.x &&
                        angle < puzzleAngles.y &&
                        angle < puzzleAngles.z &&
                        angle > -puzzleAngles.x &&
                        angle > -puzzleAngles.y &&
                        angle > -puzzleAngles.z
                       )
                    {
                        isScanned = false;
                    }
                    break;


                default:
                    break;
            }


            isScanned = true;
            //OnObjectGetScanned?.Invoke();
        }
    }
}
