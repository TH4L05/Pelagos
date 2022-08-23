using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PelagosProject.UI.Menu.Ingame
{
    public class ButtonBarMain : ButtonBar
    {
        [SerializeField] protected List<Button> panelButtons = new List<Button>();
        [SerializeField] protected bool menuButtonsEnabled;
        [SerializeField] protected bool invokeActionOnPanelChange = false;

        private void Update()
        {
            if (!isActive) return;
            NextPanel();
            PreviousPanel();
        }

        private void OnDisable()
        {
            isActive = false;
            DisableAllPanels();
        }

        private void NextPanel()
        {
            if (!menuButtonsEnabled) return;
            if (playerInput && playerInput.UINavigateNextActive)
            {
                playerInput.UINavigateNextActive = false;
                currentPanelIndex++;

                if (currentPanelIndex > panels.Count - 1)
                {
                    currentPanelIndex = 0;
                }
                DisableAllPanels();
                ActivatePanel(currentPanelIndex);
                if (panelButtons.Count != 0 && currentPanelIndex < panelButtons.Count) panelButtons[currentPanelIndex].Select();
                lastIndex = currentPanelIndex;
            }
        }

        private void PreviousPanel()
        {
            if (!menuButtonsEnabled) return;
            if (playerInput && playerInput.UINavigatePreviousActive)
            {
                playerInput.UINavigatePreviousActive = false;
                currentPanelIndex--;

                if (currentPanelIndex < 0)
                {
                    currentPanelIndex = panels.Count - 1;
                }
                DisableAllPanels();
                ActivatePanel(currentPanelIndex);
                if (panelButtons.Count != 0 && currentPanelIndex < panelButtons.Count) panelButtons[currentPanelIndex].Select();
                lastIndex = currentPanelIndex;
            }
        }
    }
}

