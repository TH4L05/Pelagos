

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PelagosProject.User.Input;
using PelagosProject.UI.Menu.Ingame;

namespace PelagosProject.UI.Menu
{
    public class OptionsSection : MonoBehaviour
    {
        [SerializeField] private List<Button> subsectionButtons = new List<Button>();
        [SerializeField] private bool enableBack = false;
        [SerializeField] private IngameMenuPanel rootPanel;
        //[SerializeField] private ButtonBar rootButtonBar;
        [SerializeField] private GameObject[] uiInputInfoObjects;
 
        private int lastactiveIndex;
        private bool isActive;
        private PlayerInput input;

        private void OnEnable()
        {
            lastactiveIndex = 0;
            input = Game.Instance.Input;
            SetState(true);
            ChangeInputStatus(true);

        }

        private void OnDisable()
        {
            lastactiveIndex = -1;
            SetState(false);
        }

        private void Update()
        {
            if (!isActive) return;
            if (!enableBack) return;

            if (input.UIoptionsNavigatBackActive)
            {
                input.UIoptionsNavigatBackActive = false;

                SetState(false);
                ChangeInputStatus(false);
                if (rootPanel == null) return;
                rootPanel.SelectButton();
            }
        }

        public void ChangeInputStatus(bool active)
        {
            input.SetUiOptionsInputActionsStatus(active);
            input.SetExtraInputActionsStatus(!active);
        }

        public void SetState(bool active)
        {
            isActive = active;
            
        }

        public void SetIndex(int index)
        {
            lastactiveIndex = index;
        }

        public void SelectButton()
        {
            if (subsectionButtons.Count < 1) return;
            if(lastactiveIndex > subsectionButtons.Count -1) return;
            StartCoroutine(Select());
        }

        IEnumerator Select()
        {
            yield return new WaitForEndOfFrame();
            subsectionButtons[lastactiveIndex].Select();
        }

        public void ChangeUIInputinfoOnjectsStatus(bool active)
        {
            if(uiInputInfoObjects.Length < 1) return;

            foreach (var obj in uiInputInfoObjects)
            {
                obj.SetActive(active);
            }
        }

    }
}

