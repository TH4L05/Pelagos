/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace PelagosProject.UI
{
    public class UIInputActionInfo : MonoBehaviour
    {
        #region Fields

        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private string textprefix;
        [SerializeField] private string textsuffix;
        [SerializeField] private bool updateOnEnable = false;
        private bool joystickActive;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            PelagosProject.User.Input.PlayerInput.SpeedlinkJoystickDeviceChanged += JoystickChanged;
        }

        private void OnDestroy()
        {
            PelagosProject.User.Input.PlayerInput.SpeedlinkJoystickDeviceChanged -= JoystickChanged;
        }

        private void OnEnable()
        {
            if (updateOnEnable)
            {
                joystickActive = Game.Instance.Input.SpeedLinkJoystickConnected;
                UpdateTextInfo();
            }
        }

        #endregion

        private void JoystickChanged(bool active)
        {
            joystickActive = active;
            UpdateTextInfo();
        }

        private void UpdateTextInfo()
        {
            if (textField == null) return;
            if (inputActionReference == null) return;

            if (joystickActive)
            {
                if (Game.Instance.Input.ConnectedJoystickVersion == 0)
                {
                    textField.text = $"{textprefix} {inputActionReference.action.bindings[0].ToDisplayString()} {textsuffix}";
                }
                else if (Game.Instance.Input.ConnectedJoystickVersion == 1)
                {
                    textField.text = $"{textprefix} {inputActionReference.action.bindings[1].ToDisplayString()} {textsuffix}";
                }
                else
                {
                    textField.text = $"{textprefix} {inputActionReference.action.bindings[0].ToDisplayString()} {textsuffix}";
                }
            }
            else
            {
                if (inputActionReference.action.bindings.Count > 2)
                {
                    textField.text = textprefix + inputActionReference.action.bindings[2].ToDisplayString() + textsuffix;
                }
                else
                {
                    textField.text = textprefix + inputActionReference.action.bindings[1].ToDisplayString() + textsuffix;
                }

            }
        }
    }
}

