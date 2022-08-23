using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PelagosProject.UI
{
    public class UIInputActionIcon : MonoBehaviour
    {
        #region Fields

        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Image infoImage;
        [SerializeField] private string keyboardText;
        [SerializeField] private string joystickText;
        private bool joystickActive;
        public Sprite joystickIcon;
        public Sprite keyboardIcon;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            PelagosProject.User.Input.PlayerInput.SpeedlinkJoystickDeviceChanged += JoystickChanged;
            joystickActive = Game.Instance.Input.SpeedLinkJoystickConnected;
            UpdateTextInfo();          
        }

        private void OnDisable()
        {
            PelagosProject.User.Input.PlayerInput.SpeedlinkJoystickDeviceChanged -= JoystickChanged;
        }

        #endregion

        private void JoystickChanged(bool active)
        {
            joystickActive = active;
            UpdateTextInfo();
        }

        private void UpdateTextInfo()
        {
            if (joystickActive)
            {
                infoImage.sprite = joystickIcon;
                textField.text = joystickText;
            }
            else
            {
                infoImage.sprite = keyboardIcon;
                textField.text = keyboardText;
            }
        }
    }
}

    
