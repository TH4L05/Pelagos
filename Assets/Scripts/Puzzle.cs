/// <author>Thoams Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PelagosProject.Interactables;
using System;

namespace PelagosProject.Puzzles
{
    public class Puzzle : MonoBehaviour
    {
        public static Action<Sprite> PuzzleStarted;
        public static Action PuzzleComplete;

        [SerializeField] private List<PuzzleInteractable> puzzleReciverInteractables = new List<PuzzleInteractable>();
        [SerializeField] private PuzzleInteractable puzzleStartInteractable;
        [SerializeField] private UnityEvent OnAllPuzzleReciveObjectScanned;
        public bool puzzleStartet = false;
        public bool puzzleFinished = false;
        public bool inZone;

        void Start()
        {
            InPuzzleZone(false);
            InvokeRepeating("PuzzleStartCheck", 0f, 0.25f);
        }


        void PuzzleStartCheck()
        {
            if (puzzleStartInteractable.isScanned)
            {
                CancelInvoke("PuzzleStartCheck");
                puzzleStartet = true;
                PuzzleStarted?.Invoke(puzzleStartInteractable.PuzzleSprite);
                InvokeRepeating("CheckAllReciversAreScanned", 0f, 0.2f);
            }
            
        }

        public void InPuzzleZone(bool inZone)
        {
            this.inZone = inZone;

            foreach (var puzzleReciver in puzzleReciverInteractables)
            {
                puzzleReciver.isScannable = inZone;
            }
        }

        /*private void LateUpdate()
        {
            if (puzzleFinished) return;
            if (!puzzleStartet) return;
            CheckAllReciversAreScanned(); 
        }*/

        public void CheckAllReciversAreScanned()
        {
            if (puzzleFinished) return;

            bool done = false;
            foreach (var puzzleReciver in puzzleReciverInteractables)
            {
                if (puzzleReciver.PuzzleInteractableType == PuzzleInteractableType.PuzzleReciveObject)
                {
                    done = puzzleReciver.isScanned;
                }
            }

            //if (!inZone) return;
            if (!done) return;

            puzzleFinished = true;
            Debug.Log("PuzzleFINISHED");
            PuzzleComplete?.Invoke();
            OnAllPuzzleReciveObjectScanned?.Invoke();
            CancelInvoke("CheckAllReciversAreScanned");
        }

    }
}

