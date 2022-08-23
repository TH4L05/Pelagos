/// <author>Thoams Krahl</author>

using PelagosProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PelagosProject.Save;

namespace PelagosProject.UI
{
    public class PhotoGallery : MonoBehaviour
    {
        #region SerializedFields

        [Header("Slots")]
        [SerializeField] private List<PhotoGallerySlot> photoGallerySlots = new List<PhotoGallerySlot>();

        [Header("Options")]
        [SerializeField] private string screenshotFolderName = "Screenshots";
        [SerializeField] private bool usePersitentDataPath = false;

        #endregion;

        #region PrivateFields

        private List<Sprite> savedScreenShots = new List<Sprite>();
        private PhotoMode photoMode;
        private PhotoGalleryFullScreenView photoGalleryFullScreenView;
        private string path;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            PhotoGalleryUI.PhotoGalleryUIStatusChanged += UIStatusChanged;
            PhotoGallerySlot.ShowInFullView += ShowInFullView;
            PhotoGallerySlot.DeleteAScreenshot += DeleteScreenshot;
            GetComponents();
            path = SetPath();
        }

        private void OnDestroy()
        {
            PhotoGalleryUI.PhotoGalleryUIStatusChanged -= UIStatusChanged;
            PhotoGallerySlot.ShowInFullView -= ShowInFullView;
            PhotoGallerySlot.DeleteAScreenshot -= DeleteScreenshot;
        }

        private void Start()
        {
            if (photoMode != null) photoMode.Setup(screenshotFolderName, usePersitentDataPath, photoGallerySlots.Count, savedScreenShots.Count);
        }

        #endregion

        #region Setup

        private void GetComponents()
        {
            photoMode = GetComponent<PhotoMode>();
            photoGalleryFullScreenView = GetComponent<PhotoGalleryFullScreenView>();
        }

        private string SetPath()
        {
            string path = string.Empty;

            if (usePersitentDataPath)
            {
                path = Application.persistentDataPath + "/" + screenshotFolderName + "/";
            }
            else
            {
                path = screenshotFolderName + "/";
            }

            return path;
        }

        private void UIStatusChanged(bool enabled)
        {
            if (enabled)
            {
                LoadFromDisk();
            }
            else
            {

            }
        }

        public void LoadFromDisk()
        {
            if (photoGallerySlots.Count == 0) return;

            savedScreenShots.Clear();
            string[] screenshots = Serialization.GetFilesFromDirectory(path, "*.png");

            for (int i = 0; i < screenshots.Length; i++)
            {
                Texture2D screenshot = new Texture2D(2, 2);
                byte[] data = Serialization.LoadFromFileByteArray(screenshots[i]);
                bool success = screenshot.LoadImage(data);
                if (!success) continue;
                screenshot.name = "texture_" + screenshots[i];
                Sprite sp = Sprite.Create(screenshot, new Rect(0f, 0f, screenshot.width, screenshot.height), new Vector2(0.5f, 0.5f));
                sp.name = screenshots[i];
                savedScreenShots.Add(sp);

            }
            SetSpritesInGallerySlots();
        }

        #endregion


        #region Slots

        private void ResetAllSlots()
        {
            foreach (var photoGallerySlot in photoGallerySlots)
            {
                photoGallerySlot.ResetSlot();
            }
        }

        private void SetSpritesInGallerySlots()
        {
            ResetAllSlots();
            if (savedScreenShots.Count == 0) return;

            for (int i = 0; i < savedScreenShots.Count; i++)
            {
                StartCoroutine(SetSprite(i));
            }
        }

        IEnumerator SetSprite(int i)
        {
            yield return new WaitForEndOfFrame();
            photoGallerySlots[i].SetSprite(savedScreenShots[i]);
        }

        #endregion

        private void ShowInFullView(Sprite sprite)
        {
            if(sprite == null) return;
            if (photoGalleryFullScreenView == null) return;
            
            photoGalleryFullScreenView.SetSprite(sprite);
            photoGalleryFullScreenView.ShowUIObject();
        }

        private void DeleteScreenshot(Sprite sprite)
        {
            Debug.Log("DeleteAScreenshot");
            if (sprite == null) return;

            foreach (var savedScreenshot in savedScreenShots)
            {
                if (savedScreenshot == sprite)
                {
                    string file = savedScreenshot.name;
                    savedScreenShots.Remove(savedScreenshot);
                    Serialization.DeleteFile(file);
                    Debug.Log($"Screenshot: {savedScreenshot.name}  = Deleted");
                    break;
                }
            }
            SetSpritesInGallerySlots();
        }
    }
}

