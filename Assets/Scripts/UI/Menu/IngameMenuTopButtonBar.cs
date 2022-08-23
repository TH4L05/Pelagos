/// <author>Thoams Krahl</author>

using PelagosProject.User.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PelagosProject.UI.Menu.Ingame
{
    public class IngameMenuTopButtonBar : MonoBehaviour
    {
        #region Events

        public Action<int> ActiveSectionChanged;

        #endregion

        #region SerializedFields

        [SerializeField] private List<IngameMenuTopBarButton> panelButtons = new List<IngameMenuTopBarButton>();
        [SerializeField] private bool invokeActionOnPanelChange = false;

        [Tooltip("Set Index to -1 to disable panel activation on ennable")]
        [SerializeField] private int startPanelIndex = -1;

        #endregion

        #region PrivateFields

        private PlayerInput playerInput;
        private int panelIndex;
        private int lastIndex = 0;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            playerInput = Game.Instance.Input;
            panelIndex = startPanelIndex;
            lastIndex = panelIndex;
        }

        private void OnEnable()
        {
            DisableAllPanels();
            if (startPanelIndex == -1) return;
            panelButtons[panelIndex].ActivatePanel();
        }

        private void Update()
        {
            NextPanel();
            PreviousPanel();
        }

        private void OnDisable()
        {
            DisableAllPanels();
        }

        #endregion

        private void NextPanel()
        {
            if (playerInput && playerInput.UINavigateNextActive)
            {
                playerInput.UINavigateNextActive = false;
                panelIndex++;

                if (panelIndex > panelButtons.Count - 1)
                {
                    panelIndex = 0;
                }
                DisableAllPanels();
                panelButtons[panelIndex].ActivatePanel();
                lastIndex = panelIndex;

            }
        }

        private void PreviousPanel()
        {
            if (playerInput && playerInput.UINavigatePreviousActive)
            {
                playerInput.UINavigatePreviousActive = false;
                panelIndex--;

                if (panelIndex < 0)
                {
                    panelIndex = panelButtons.Count - 1;
                }
                DisableAllPanels();
                panelButtons[panelIndex].ActivatePanel();
                lastIndex = panelIndex;
            }
        }

        public void DisableAllPanels()
        {
            foreach (var panels in panelButtons)
            {
                panels.DisablePanel();
            }
        }
    }
}

