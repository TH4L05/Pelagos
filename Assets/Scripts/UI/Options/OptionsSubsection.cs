using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PelagosProject.User.Input;
using UnityEngine.EventSystems;

namespace PelagosProject.UI.Menu
{
    public class OptionsSubsection : MonoBehaviour
    {
        public UnityEvent activateEvent;
        public UnityEvent deactivateEvent;

        public UnityEvent leftButtonEvent;
        public UnityEvent rightButtonEvent;

        [SerializeField] private Image activeImage;
        [SerializeField] private OptionsSection rootSectionComponent;
        [SerializeField, Range(0f, 1f)] private float clickwaitTime = 0.25f;
        [SerializeField] private GameObject[] uiInputInfoObjects;
        [SerializeField] private EventSystem eventSystem;

        private bool isActive;
        private int index;
        private PlayerInput input;
        private bool canClick = true;

        private void OnEnable()
        {
            input = Game.Instance.Input;
        }

        private void Update()
        {
            if (!isActive) return;
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (input.UIoptionsNavigatBackActive)
            {
                Debug.Log("InputBackPressed");
                input.UIoptionsNavigatBackActive = false;
                Deactivate();             
                return;
            }

            if (input.UIoptionsNavigatLeftActive)
            {
                if (!canClick) return;
                canClick = false;
                Debug.Log("InputLeftPressed");
                InvokeLeftEvent();
                return;
            }

            if (input.UIoptionsNavigatRightActive)
            {
                if (!canClick) return;
                canClick = false;
                Debug.Log("InputRightPressed");
                InvokeRightEvent();
                return;
            }
        }

        IEnumerator NextInputWait()
        {
            Debug.Log("Wait start");
            yield return new WaitForSecondsRealtime(clickwaitTime);
            Debug.Log("Wait done");
            canClick = true;
        }

        public void Activate()
        {
            if (isActive) return;
            Debug.Log("Activate");
            isActive = true;
            activateEvent?.Invoke();
            ChangeUIInputinfoOnjectsStatus(true);
            Game.Instance.EventSystem.enabled = false;
            activeImage.gameObject.SetActive(true);
            input.SetExtraInputActionsStatus(false);
            input.SetUIInputActionsStatus(false);
            input.SetUiOptionsInputActionsStatus(true);

        }

        public void Deactivate()
        {
            if (!isActive) return;
            Debug.Log("Decativate");
            isActive = false;
            deactivateEvent?.Invoke();
            ChangeUIInputinfoOnjectsStatus(false);
            Game.Instance.EventSystem.enabled = true;
            input.SetUIInputActionsStatus(true);
            activeImage.gameObject.SetActive(false);
            rootSectionComponent.SelectButton();
        }

        public void ChangeStatus(bool active)
        {
            isActive = active;
        }

        public void InvokeLeftEvent()
        {
            if (!isActive) return;
            leftButtonEvent?.Invoke();
            StartCoroutine(NextInputWait());
        }

        public void InvokeRightEvent()
        {
            if (!isActive) return;
            rightButtonEvent?.Invoke();
            StartCoroutine(NextInputWait());
        }

        public void ChangeUIInputinfoOnjectsStatus(bool active)
        {
            if (uiInputInfoObjects.Length < 1) return;

            foreach (var obj in uiInputInfoObjects)
            {
                obj.SetActive(active);
            }
        }



    }
}

