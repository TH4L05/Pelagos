/// <author>Thoams Krahl</author>

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PelagosProject.Interactables;

namespace PelagosProject.UI.Menu.Ingame
{
    public class ScannableLibaryUISlot : MonoBehaviour
    {
        #region Events

        public static Action<ScannableData> ShowInformations;

        #endregion

        #region SerializedFields

        [SerializeField] private int libaryIndex;
        [SerializeField] private TextMeshProUGUI buttonTextField;
        [SerializeField] private Button slotButton;

        #endregion

        private ScannableData slotdata;
        public bool isInteractable = false;

        #region UnityFunctions

        private void OnEnable()
        {
            slotButton.interactable = isInteractable;
            //OnSlotGetsEneabled();
        }

        private void OnDisable()
        {
            
        }

        #endregion


        public void SelectButton()
        {
            slotButton.Select();
        }

        public void SetIndex(int index)
        {
            libaryIndex = index;
            OnSlotGetsEneabled();
        }

        public void OnSlotGetsEneabled()
        {
            GetSlotDataFromLibary();

            if (slotdata == null)
            {
                Debug.LogWarning($"LibarySlot at Index {libaryIndex} could not be Updated -> SlotData == NULL");
                return;
            }

            if (slotdata.IsUnlocked)
            {
                //buttonTextField.gameObject.SetActive(true);
                buttonTextField.text = slotdata.InteractableName;
            }
            else
            {
                //buttonTextField.gameObject.SetActive(false);
                buttonTextField.text = libaryIndex + "\n Unkown Entry";
            }

        }
        private void GetSlotDataFromLibary()
        {
            slotdata = ScannableLibary.Instance.GetScannableData(libaryIndex);
        }

        public void ShowInfo()
        {       
            ShowInformations?.Invoke(slotdata);
        }

        public void ShowInfo(int idx)
        {
            libaryIndex = idx;
            GetSlotDataFromLibary();
            ShowInformations?.Invoke(slotdata);
        }

    }
}

