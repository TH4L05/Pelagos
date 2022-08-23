/// <author>Thoams Krahl</author>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;
using PelagosProject.Data;

namespace PelagosProject.UI.Menu
{
    public class Options : MonoBehaviour
    {
        #region SerializedFields

        [Header("Data")]
        [SerializeField] private SettingsData data;
        public InputActionAsset inputAsset;

        [Header("FillIamges")]
        [SerializeField] private Image masterVolumeFillImage;
        [SerializeField] private Image musicVolumeFillImage;
        [SerializeField] private Image sfxVolumeFillImage;
        [SerializeField] private float masterVolumeMaxValue = 100.0f;
        [SerializeField] private float musicVolumeMaxValue = 100.0f;
        [SerializeField] private float sfxVolumeMaxValue = 100.0f;
        //[SerializeField] private Image gammaFillImage;

        [Header("FillValueTexts")]
        [SerializeField] private TextMeshProUGUI masterVolumeText;
        [SerializeField] private TextMeshProUGUI musicVolumeText;
        [SerializeField] private TextMeshProUGUI sfxVolumeText;
        //[SerializeField] private TextMeshProUGUI gammaText;

        [Header("OptionTexts")]
        [SerializeField] private TextMeshProUGUI fullScreenOptionText;
        [SerializeField] private TextMeshProUGUI vsyncOptionText;
        [SerializeField] private TextMeshProUGUI resolutionOptionText;
        [SerializeField] private TextMeshProUGUI qualtiyOptionText;

        #endregion

        #region Private Fields

        private string[] qualityLevel;
        private float masterVolume = 1f;
        private float musicVolume = 1f;
        private float sfxVolume = 1f;
        private int currentResolutionIndex = 0;
        private int currentResolutionWidth = 640;
        private int currentResolutionHeight = 480;
        Resolution[] resolutions;
        private bool fullScreen = true;
        private bool vsync = true;
        private int currentQualityLevel = 0;

        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;

        private VCA masterVCA;

        #endregion

        #region Unity Functions

        private void Awake()
        {
            //Setup();
        }

        private void OnDisable()
        {
            Save();
        }

        #endregion

        #region Setup

        public void Setup()
        {
            Debug.Log("OptionsSetup");
            resolutions = Screen.resolutions;
            qualityLevel = QualitySettings.names;

            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");

            LoadData();
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
            SetFullScreen();
            SetVsync();     
            SetQuality();

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    data.SetResolutionValues(resolutions[i].width, resolutions[i].height, currentResolutionIndex);
                }
            }

            SetResolution();

        }


        private void CheckCurrentResolution()
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    currentResolutionWidth = resolutions[i].width;
                    currentResolutionHeight = resolutions[i].height;
                    data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
                    SetResolution();
                    return;
                }
            }

            if (currentResolutionIndex == 0)
            {
                SetDefaultResolution();
            }
        }

        private void SetDefaultResolution()
        {
            currentResolutionWidth = data.dfaultCurrentResolution_width;
            currentResolutionHeight = data.dfaultCurrentResolution_height;
            //Screen.SetResolution(currentResolutionWidth, currentResolutionHeight, true);
            CheckCurrentResolution();
        }

        #endregion

        #region Change Options

        public void ChangeMasterVolume(float volumeValue)
        {
            masterVolume += volumeValue;
            if (masterVolume < 0)
            {
                masterVolume = 0;
            }

            if (masterVolume > masterVolumeMaxValue)
            {
                masterVolume = masterVolumeMaxValue;
            }

            data.SetMasterVolume(masterVolume);
            SetMasterVolume();
        }

        public void ChangeMusicVolume(float volumeValue)
        {
            musicVolume += volumeValue;
            if (musicVolume < 0)
            {
                musicVolume = 0;
            }

            if (musicVolume > musicVolumeMaxValue)
            {
                musicVolume = musicVolumeMaxValue;
            }


            data.SetMusicVolume(musicVolume);
            SetMusicVolume();
        }

        public void ChangeSFXVolume(float volumeValue)
        {
            sfxVolume += volumeValue;
            if (sfxVolume < 0)
            {
                sfxVolume = 0;
            }

            if (sfxVolume > sfxVolumeMaxValue)
            {
                sfxVolume = sfxVolumeMaxValue;
            }

            data.SetSFXVolume(sfxVolume);
            SetSFXVolume();
        }

        public void ChangeQualityMinus()
        {
            if (currentQualityLevel > 0) currentQualityLevel--;
            data.SetQualityLevelIndex(currentQualityLevel);
            SetQuality();
        }

        public void ChangeQualityPlus()
        {
            if (currentQualityLevel < qualityLevel.Length - 1) currentQualityLevel++;
            data.SetQualityLevelIndex(currentQualityLevel);
            SetQuality();
        }

        public void ChangeResolutionMinus()
        {
            if (currentResolutionIndex > 0)
            {
                currentResolutionIndex--;
            }
            else
            {
                currentResolutionIndex = resolutions.Length - 1;
            }

            currentResolutionWidth = resolutions[currentResolutionIndex].width;
            currentResolutionHeight = resolutions[currentResolutionIndex].height;
            data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
            SetResolution();
        }

        public void ChangeResolutionPlus()
        {
            if (currentResolutionIndex < resolutions.Length - 1)
            {
                currentResolutionIndex++;
            }
            else
            {
                currentResolutionIndex = 0;
            }

            currentResolutionWidth = resolutions[currentResolutionIndex].width;
            currentResolutionHeight = resolutions[currentResolutionIndex].height;
            data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
            SetResolution();
        }

        public void ChangeFullScreen(bool isFullscreen)
        {
            fullScreen = isFullscreen;
            data.SetFullscreenStatus(fullScreen);
            SetFullScreen();
        }

        public void ChangeVsync(bool isVsync)
        {
            vsync = isVsync;
            data.SetVsyncStatus(vsync);
            SetVsync();
        }

        /*public void ChangeGamma(float value)
        {
            LiftGammaGain lgg;

            if (gammaProfile.TryGet(out lgg))
            {
                var gvalue = (value / 10);
                var newGamma = new Vector4(1, 1, 1, gvalue);
                lgg.gamma.Override(newGamma);
                //lgg.lift.Override(newGamma);
                //lgg.gain.Override(newGamma);
            }

            //Debug.Log(lgg.gamma + "/" + lgg.lift + "/" + lgg.gain);

            if (gammaSliderText) gammaSliderText.text = value.ToString();
        }*/

        #endregion

        #region Set Options

        public void SetMasterVolume()
        {
          
            masterVolume = data.MasterVolume;
            if (masterVolumeFillImage) masterVolumeFillImage.fillAmount = masterVolume / masterVolumeMaxValue;
            if (masterVolumeText) masterVolumeText.text = masterVolume.ToString("000");
            //masterBus.setVolume(masterVolume / masterVolumeMaxValue);
            masterBus.setVolume(masterVolume / masterVolumeMaxValue);
        }

        public void SetMusicVolume()
        {
            musicVolume = data.MusicVolume;
            if (musicVolumeFillImage) musicVolumeFillImage.fillAmount = musicVolume / musicVolumeMaxValue;
            if (musicVolumeText) musicVolumeText.text = musicVolume.ToString("000");
            musicBus.setVolume(musicVolume / musicVolumeMaxValue);
        }

        public void SetSFXVolume()
        {
            sfxVolume = data.SfxVolume;
            if (sfxVolumeFillImage) sfxVolumeFillImage.fillAmount = sfxVolume / sfxVolumeMaxValue;
            if (sfxVolumeText) sfxVolumeText.text = sfxVolume.ToString("000");

        }

        public void SetFullScreen()
        {
            fullScreen = data.FullScreen;
            Screen.fullScreen = fullScreen;

            if (fullScreen)
            {
                fullScreenOptionText.text = "ON";
                Screen.fullScreen = true;
            }
            else
            {
                fullScreenOptionText.text = "OFF";
                Screen.fullScreen = false;
            }
        }

        public void SetVsync()
        {
            vsync = data.VSync;
            if (vsync)
            {
                vsyncOptionText.text = "ON";
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                vsyncOptionText.text = "OFF";
                QualitySettings.vSyncCount = 0;
            }
        }

        public void SetResolution()
        {
            currentResolutionIndex = data.CurrentResolutionIndex;
            currentResolutionWidth = data.CurrentResolution_width;
            currentResolutionHeight = data.CurrentResolution_height;        
            resolutionOptionText.text = currentResolutionWidth + " x " + currentResolutionHeight + " , " + resolutions[currentResolutionIndex].refreshRate;
            Screen.SetResolution(currentResolutionWidth, currentResolutionHeight, fullScreen);
        }

        public void SetQuality()
        {
            currentQualityLevel = data.QualityLevelIndex;
            //QualitySettings.SetQualityLevel(currentQualityLevel);
            //qualtiyOptionText.text = qualityLevel[currentQualityLevel];
        }

        #endregion


        //*********************************************
        // InputAction Rebind are unused in Project
        //*********************************************


        // TODO: Create separate Region for Rebind
        public void Save()
        {
            //string binds = inputAsset.SaveBindingOverridesAsJson();
            //data.SetBindings(binds);
            data.SaveValues();
        }

        public void LoadData()
        {
            data.LoadValues();
            /*if (data.InputActionRebindings != string.Empty)
            {
                inputAsset.LoadBindingOverridesFromJson(data.InputActionRebindings);
            }*/

        }

        public void LoadDefaults()
        {
            data.SetDefaultValues();
            SetSFXVolume();
            SetMusicVolume();
            SetMasterVolume();
            SetFullScreen();
            SetVsync();
            SetResolution();
            SetQuality();
            //inputAsset.RemoveAllBindingOverrides();
            //Serialization.DeleteFile("rebindings.data");
        }      
    }
}

