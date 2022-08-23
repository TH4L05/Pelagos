using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PelagosProject.User.Input;

namespace PelagosProject.UI.Menu
{
    public class OptionsControlsSection : MonoBehaviour
    {
        private bool speedlinkJoystickInUse;
        [SerializeField] private GameObject controllerUIObj;
        [SerializeField] private GameObject keyboardUIObj;


        private void OnEnable()
        {
            PlayerInput.SpeedlinkJoystickDeviceChanged += SpeedLinkJoystickStatusChanged;
            GetCurrentSpeedlinkJoystickStatus();
        }

        private void OnDisable()
        {
            PlayerInput.SpeedlinkJoystickDeviceChanged -= SpeedLinkJoystickStatusChanged;
        }

        private void SpeedLinkJoystickStatusChanged(bool active)
        {
            speedlinkJoystickInUse = active;
            SetObjectStatus();
        }

        private void GetCurrentSpeedlinkJoystickStatus()
        {
            speedlinkJoystickInUse = Game.Instance.Input.SpeedLinkJoystickConnected;

            SetObjectStatus();
        }

        private void SetObjectStatus()
        {
            if (speedlinkJoystickInUse)
            {
                if(controllerUIObj != null) controllerUIObj.SetActive(true);
                if(keyboardUIObj != null) keyboardUIObj.SetActive(false);
            }
            else
            {
                if (controllerUIObj != null) controllerUIObj.SetActive(false);
                if (keyboardUIObj != null) keyboardUIObj.SetActive(true);
            }
        }

    }
}

