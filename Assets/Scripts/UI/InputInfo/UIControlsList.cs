/// <author>Thoams Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using PelagosProject.UI.Menu.Ingame;


namespace PelagosProject.UI
{
    public class UIControlsList : MonoBehaviour
    {
        [SerializeField] private List<ControlsInputInfo> controlsInfoList = new List<ControlsInputInfo>();
        [SerializeField] private bool updateOnEnable = false;

        private void OnEnable()
        {
            if (!updateOnEnable) return;
            UpdateSlots();
        }

        public void UpdateSlots()
        {
            if (controlsInfoList.Count == 0) return;
            foreach (var item in controlsInfoList)
            {
                item.Setup();
            }
        }
    }
}

