/// <author>Thoams Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using PelagosProject.User.Input;
using UnityEngine.UI;

namespace PelagosProject.UI
{
    public class ButtonBar : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] protected List<GameObject> panels = new List<GameObject>();
        [SerializeField] protected bool activateFirstPanelOnEnable = false;

        #endregion

        #region PrivateFields

        protected bool isActive;
        protected int lastIndex = 0;
        protected PlayerInput playerInput;
        protected int currentPanelIndex;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            playerInput = Game.Instance.Input;
            currentPanelIndex = 0;
            lastIndex = currentPanelIndex;
        }

        private void OnEnable()
        {
            isActive = true;
            DisableAllPanels();
            if (!activateFirstPanelOnEnable) return;
            ActivatePanel(currentPanelIndex);
                 
        }

        #endregion

        public void DisableAllPanels()
        {
            foreach (var panels in panels)
            {
                panels.SetActive(false);
            }
        }

        public void ActivatePanel(int index)
        {         
            lastIndex = index;
            panels[index].SetActive(true);
        }

        public void DisablePanel(int index)
        {
            lastIndex = index;
            panels[index].SetActive(false);
        }
    }
}

