/// <author>Thoams Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PelagosProject.User.Input;

namespace PelagosProject.UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button firstSelectedButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI joystickInfoTextField;
        private bool speedlinkJoystickActive;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
        }

        public void Start()
        {
            PlayerInput.SpeedlinkJoystickDeviceChanged += SpeedlinkJoystickActive;
            SelectFirstButton();
        }

        private void OnDestroy()
        {
            PlayerInput.SpeedlinkJoystickDeviceChanged -= SpeedlinkJoystickActive;
        }


        #endregion

        private void SpeedlinkJoystickActive(bool active)
        {
            speedlinkJoystickActive = active;
            if (joystickInfoTextField) 
                
            if (speedlinkJoystickActive)
            {
                joystickInfoTextField.text = $"speedlink Joystick connected (V{Game.Instance.Input.ConnectedJoystickVersion})";
            }
            else
            {
                    joystickInfoTextField.text = $"No speedlink Joystick connected";
                }
        }

        private void SetContinueButtonStatus()
        {
            if (Game.Instance.PlayerProfileSlot == null) return;
            if (Game.Instance.PlayerProfileSlot.playerProfile == null) return;
            if (Game.Instance.PlayerProfileSlot.playerProfile.startedOnce)
            {
                Debug.Log("PlayerStartetOnce");
                continueButton.interactable = true;
                firstSelectedButton = continueButton;
            }
        }

        public void SelectFirstButton()
        {
            Debug.Log("SelectFirstButton");
            StartCoroutine(SelectButton());
        }

        IEnumerator Setup()
        {
            yield return new WaitForSeconds(1f);
            SetContinueButtonStatus();
            
        }

        IEnumerator SelectButton()
        {
            yield return new WaitForEndOfFrame();
            if (firstSelectedButton != null) firstSelectedButton.Select();
        }

        public void Quit()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
    }
#endif
    }
}

