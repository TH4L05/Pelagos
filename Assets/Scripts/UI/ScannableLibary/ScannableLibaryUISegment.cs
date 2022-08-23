/// <author>Thoams Krahl</author>
///source = https://pavcreations.com/scrollable-menu-in-unity-with-button-or-key-controller/#position-animations


using PelagosProject.User.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PelagosProject.UI.Menu.Ingame
{
    public class ScannableLibaryUISegment : MonoBehaviour
    {
        [SerializeField] private List<ScannableLibaryUISlot> scannableLibaryUISlots = new List<ScannableLibaryUISlot>();
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private float size = 110f;

        [SerializeField] RectTransform rectTransform;
        private int index;
        private int verticalMovement = 0;

        private int maxIndex => scannableLibaryUISlots.Count - 1;

        private void OnEnable()
        {
            playerInput = Game.Instance.Input;
            UpdateSlots();       
            index = 0;
            scannableLibaryUISlots[index].ShowInfo(index);
        }

        private void OnDisable()
        {
        }

        private void Update()
        {
            CheckInput();
            SetRect();
            SelectIndex();
        }

        private void CheckInput()
        {
            verticalMovement = 0;
            if (playerInput && playerInput.UINavigatUpActive)
            {
                //playerInput.UINavigatUpActive = false;
                verticalMovement = -1;
            }

            if (playerInput && playerInput.UINavigatDownActive)
            {
                //playerInput.UINavigatDownActive = false;
                verticalMovement = 1;
            }

            if (playerInput && playerInput.UINavigatUpActive && playerInput && playerInput.UINavigatDownActive)
            {
                //playerInput.UINavigatUpActive = false;
                //playerInput.UINavigatDownActive = false;
                verticalMovement = 0;
            }
        }

        private void SetRect()
        {
            if (verticalMovement != 0)
            {
                switch (verticalMovement)
                {
                    case 1:
                        InreaseIndex();
                        break;

                    case -1:
                        DereaseIndex();
                        break;


                    default:
                        break;
                }

                
            }
        }

        private void InreaseIndex()
        {
            playerInput.UINavigatDownActive = false;
            if (index < maxIndex)
            {
                index++; 
                if (index > 1 && index < maxIndex)
                {
                    rectTransform.offsetMax -= new Vector2(0, -size);
                }
            }
            else
            {
                index = 0;
                rectTransform.offsetMax = Vector2.zero;
            }
            
        }

        private void DereaseIndex()
        {
            playerInput.UINavigatUpActive = false;
            if (index > 0)
            {
                index--;
                if (index < maxIndex && index > 0)
                {
                    rectTransform.offsetMax -= new Vector2(0, size);
                }
            }
            else
            {
                index = maxIndex;
                rectTransform.offsetMax = new Vector2(0, ((maxIndex) - 3) * size);
            }
        }

        private void SelectIndex()
        {
            if (verticalMovement == 0) return;
            scannableLibaryUISlots[index].SelectButton();
            scannableLibaryUISlots[index].ShowInfo(index);        
        }


        /*private void Sroll()
        {
            float i = index;
            float v = scannableLibaryUISlots.Count;

            float v1 = i / v;
            float value = 1 - v1;
            Debug.Log("TEST = " + v1 + " / " + value);

            //scrollbar.value = value;
            m_NextScrollPosition = new Vector2(0, value);
        }*/


        private void UpdateSlots()
        {
            int slotIndex = 0;
            foreach (var slot in scannableLibaryUISlots)
            {
                slot.SetIndex(slotIndex);
                slotIndex++;
            }
        }
    }
}

