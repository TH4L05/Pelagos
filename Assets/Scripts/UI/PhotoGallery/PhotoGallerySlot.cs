/// <author>Thoams Krahl</author>

using PelagosProject.User.Input;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PelagosProject.UI
{
    public class PhotoGallerySlot : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public static Action<Sprite> DeleteAScreenshot;
        public static Action<Sprite> ShowInFullView;

        private Sprite slotSprite;
        [SerializeField] private Sprite defaultSlotSprite;
        [SerializeField] private Image selectionFrame;
        [SerializeField] private Image slotImage;
        [SerializeField] private Button slotButton;
        private PlayerInput input;
        public bool selected;

        private void Awake()
        {
            if (slotImage == null) return;
            slotImage.sprite = defaultSlotSprite;
        }

        private void OnEnable()
        {
            input = Game.Instance.Input;
            
        }

        private void OnDisable()
        {
           
        }

        private void Update()
        {
            if (!selected) return;
            CheckDeleteInput();
        }


        public void OnSelect(BaseEventData eventData)
        {
            selected = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selected = false;
        }



        private void CheckDeleteInput()
        {
            if (input.PhotoGalleryDeletePictureInputActive)
            {              
                input.PhotoGalleryDeletePictureInputActive = false;
                if (slotSprite == null) return;
                if (slotSprite == defaultSlotSprite) return;
                DeleteAScreenshot?.Invoke(slotSprite);
            }
        }

        public void SetSprite(Sprite sprite)
        {
            Debug.Log("SetPhotoGallerySlotSprite");
            if (slotImage == null) return;
            if (sprite == null) return;
            slotSprite = sprite;
            slotImage.sprite = slotSprite;
        }

        public Sprite GetSlotSprite()
        {
            return slotSprite;
        }

        public void ButtonClicked()
        {
            ShowInFullView?.Invoke(slotSprite);
        }

        public void ResetSlot()
        {
            if (slotImage == null) return;
            slotImage.sprite = defaultSlotSprite;
        }

       
    }
}

