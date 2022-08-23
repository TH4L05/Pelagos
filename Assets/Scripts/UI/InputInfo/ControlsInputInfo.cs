/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace PelagosProject.UI.Menu.Ingame
{
    public class ControlsInputInfo : MonoBehaviour
    {
        #region Fields

        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private TextMeshProUGUI inputActionTextField;
        [SerializeField] private bool useManuellInputName = false;
        [SerializeField] private bool showInputActionDisplayString = false;
        [SerializeField] private bool useManuellInputActionName = false;
        [SerializeField] private string manuellInputNameText;
        [SerializeField] private string manuellInputActionText;
        [SerializeField] private int bindingsIndex = 0;

        #endregion

        public void Setup()
        {
            if (inputActionReference == null)
            {
                Debug.LogError("ERROR - InputActionReference Is Missing !!");
                return;
            }

            string name = string.Empty;
            string input = string.Empty;


            if (!useManuellInputName)
            {
                name = inputActionReference.action.name;                
            }
            else if(!string.IsNullOrEmpty(manuellInputNameText))
            {
                name = manuellInputNameText;
            }

            SetNameText(name);

            if (!showInputActionDisplayString)
            {
                inputActionTextField.gameObject.SetActive(false);
                return;
            }

            if (!useManuellInputActionName)
            {
                int bindingsCount = inputActionReference.action.bindings.Count;

                if(bindingsIndex > bindingsCount -1) bindingsIndex = bindingsCount -1;

                input = inputActionReference.action.bindings[bindingsIndex].ToDisplayString();
            }
            else
            {
                input = manuellInputActionText;
            }

            SetInputActionText(input);
        }

        private void SetNameText(string text)
        {
            if (inputActionReference == null) return;
            if (nameTextField != null) nameTextField.text = text;
        }

        private void SetInputActionText(string text)
        {
            if (inputActionReference == null) return;
            if (inputActionTextField != null) inputActionTextField.text = text;
        }

    }
}

