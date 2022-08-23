/// <author>Thoams Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PelagosProject.UI.Menu.Ingame
{
    public class IngameMenuPanel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool active = true;
        [SerializeField] private Button firstSelectedButton;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            SelectButton();
        }

        private void OnDisable()
        {
            
        }

        public void SelectButton()
        {
            if (firstSelectedButton && active) StartCoroutine(SelectFirstButton());
        }

        #endregion

        IEnumerator SelectFirstButton()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("First Button of Enabled Panel gets Selected");
            firstSelectedButton.Select();
        }
    }
}
