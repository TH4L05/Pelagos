/// <author>Thoams Krahl</author>


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PelagosProject.UI
{
    public class PhotoGalleryFullScreenView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button firstSelectedButton;
        [SerializeField] private GameObject uiObject;
        private Sprite sprite;


        public void ShowUIObject()
        {
            if (uiObject != null)
            {
                uiObject.SetActive(true);
                StartCoroutine(SelectButton());
            }
        }

        public void SetSprite(Sprite sprite)
        {
            if (sprite == null) return;
            if (image == null) return;

            this.sprite = sprite;
            image.sprite = sprite;
        }

        IEnumerator SelectButton()
        {
            yield return new WaitForEndOfFrame();
            firstSelectedButton.Select();
        }
    }
}

