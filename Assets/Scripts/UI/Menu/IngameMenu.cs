/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using PelagosProject.Audio;

namespace PelagosProject.UI.Menu.Ingame
{
    public class IngameMenu : MonoBehaviour
    {
        #region Events

        public UnityEvent TriggerEventOnMenuOpen;
        public UnityEvent TriggerEventOnMenuClose;

        #endregion

        #region Fields

        public static bool GameIsPaused;
        private enum OpenedMenuSection
        { 
            None = -1,
            Libary,
            PhotoGallery,
            Options
        }

        [SerializeField] private GameObject uiObject;
        [SerializeField] private OpenedMenuSection openedMenuSection = OpenedMenuSection.None;
        [SerializeField] private IngameMenuTopButtonBar mainButtonBar;
        [SerializeField] private bool setTimeScaleZero;

        [Header("Playables")]
        [SerializeField] private PlayableDirector showPauseMenu;
        [SerializeField] private PlayableDirector hidePauseMenu;

        [Header("Audio")]
        [SerializeField] private AudioEventList uiAudioEventList;
        
        public AudioEventList UIAudioEvents => uiAudioEventList;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            if (mainButtonBar) mainButtonBar.ActiveSectionChanged += SetOpenedMenuSection;
        }

        private void OnDisable()
        {
            if (mainButtonBar) mainButtonBar.ActiveSectionChanged -= SetOpenedMenuSection;
        }

        #endregion

        public void SetOpenedMenuSection(int sectionIndex)
        {
            openedMenuSection = (OpenedMenuSection)sectionIndex;
        }

        public void ToggleMenu()
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            uiObject.SetActive(true);
            GameIsPaused = true;
            //showPauseMenu?.Play();
            TriggerEventOnMenuOpen?.Invoke();
            if (setTimeScaleZero)
            {
                SetTimeScale(0f);
            }
            
            //Debug.Log(GamePaused);
            Game.Instance.Input.SetMovementInputActionsStatus(false);
            Game.Instance.Input.SetUIInputActionsStatus(true);
        }

        public void Resume()
        {
            uiObject.SetActive(false);
            GameIsPaused = false;
            if (setTimeScaleZero)
            {
                SetTimeScale(1f);
            }
            //hidePauseMenu.Play();     
            TriggerEventOnMenuClose?.Invoke();
            //Debug.Log(GamePaused);
            Game.Instance.Input.SetMovementInputActionsStatus(true);
            Game.Instance.Input.SetUIInputActionsStatus(false);
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
            //Debug.Log(Time.timeScale);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
    
#endif
        }
    }
}

