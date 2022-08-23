using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PelagosProject.User.Input;

namespace PelagosProject
{
    public class HowToPlay : MonoBehaviour
    {
        public UnityEvent startEvent;
        public UnityEvent closeEvent;
        private bool speedlinkJoystickInUse;

        [SerializeField] private GameObject joystickFirstPage;
        [SerializeField] private GameObject keyboardFirstPage;


        private void Awake()
        {
            PlayerInput.SetupFinished += Show;
        }

        private void OnDestroy()
        {
            PlayerInput.SetupFinished -= Show;
        }

        private void Show()
        {
            Debug.Log("SHOW How to Play");
            startEvent?.Invoke();
            speedlinkJoystickInUse = Game.Instance.Input.SpeedLinkJoystickConnected;

            if (!speedlinkJoystickInUse)
            {
                if(keyboardFirstPage != null) keyboardFirstPage.SetActive(true);
            }
            else
            {
                if(joystickFirstPage != null) joystickFirstPage.SetActive(true);
            }
        }

        public void TriggerCloseEvent()
        {
            closeEvent?.Invoke();
        }
    }
}

