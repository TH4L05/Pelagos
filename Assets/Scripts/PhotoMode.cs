/// <author>Thoams Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using PelagosProject.User.Input;
using PelagosProject.Save;

namespace PelagosProject
{
    public class PhotoMode : MonoBehaviour
    {
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private GameObject uiCanvas;
        [SerializeField] private GameObject photoModeUI;
        [SerializeField] private EventReference photoAudio;
        [SerializeField] private GameObject playerCockpit;

        private int maxScreenshotAmount;
        private bool usePersistentDataPath;
        private string screenshotFolderName;

        private int screenshotIndex;
        private bool onSave;

        [Space(5f)]
        public UnityEvent OnScreenshotButtonPressed;

        private void Awake()
        {
            PlayerInput.TakeScreenshot += ScreenShotMode;
        }

        private void OnDestroy()
        {
            PlayerInput.TakeScreenshot -= ScreenShotMode;
        }

        public void Setup(string screenshotFolderName, bool usePersistentDataPath, int galleryslotAmount, int existingShotsAmount)
        {
            this.screenshotFolderName = screenshotFolderName;
            this.usePersistentDataPath = usePersistentDataPath;
            maxScreenshotAmount = galleryslotAmount;
            screenshotIndex = existingShotsAmount;
        }

        private void ScreenShotMode()
        {
            if (!isEnabled) return;
            isEnabled = false;
            if (photoModeUI == null) return;
            if(!photoAudio.IsNull) RuntimeManager.PlayOneShot(photoAudio);
            OnScreenshotButtonPressed?.Invoke();
            
            playerCockpit.SetActive(false);
        }


        public void Cancel()
        {
            SetTimeScale(1f);
            playerCockpit.SetActive(true);
            photoModeUI.SetActive(false);
            isEnabled = true;
        }

        public void TakeScreenshot()
        {
            if (onSave) return;

            if (!onSave)
            {
                onSave = true;
            }


            //TODO: if screenshotIndex > maxScreenshotAmount => resest index and delete shot in gallery
            if (screenshotIndex > maxScreenshotAmount)
            {
                return;
            }
            StartCoroutine(TakeShot());
            
        }

        public void SetTimeScale(float scale)
        {
            if (scale < 0) return;
            Time.timeScale = scale;
        }

        private string SetPath()
        {
            string path = string.Empty;

            if (usePersistentDataPath)
            {
                path = Application.persistentDataPath + "/" + screenshotFolderName + "/";
            }
            else
            {
                path = screenshotFolderName + "/";
            }

            return path;
        }

        IEnumerator TakeShot()
        {
            uiCanvas.SetActive(false);

            yield return new WaitForEndOfFrame();

            string path = SetPath();
            Serialization.CheckDirectory(path);

            string filename = "Pelagos_Screenshot" + "_" + System.DateTime.Now.ToString("dd-MMMM-yyyy_HH-mm-ss") + ".png";
            ScreenCapture.CaptureScreenshot(path + filename);


            playerCockpit.SetActive(true);
            uiCanvas.SetActive(true);
            Debug.Log("Screenshot saved");
            screenshotIndex++;

            
            onSave = false;
            SetTimeScale(1f);
            photoModeUI.SetActive(false);
            isEnabled = true;
        }

        public void SetStatus(bool enabled)
        {
            isEnabled = enabled;
        }
    }
}

