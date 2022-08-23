/// <author>Thoams Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using PelagosProject.Save;

namespace PelagosProject.Data
{
    [CreateAssetMenu(fileName = "New Settings", menuName = "PelagosProject/Data/Settings")]
    public class SettingsData : ScriptableObject
    {
        #region Fields

        private float masterVolume = 1f;
        private float musicVolume = 1f;
        private float sfxVolume = 1f;
        private int currentResolutionIndex = 0;
        private int currentResolution_width = 640;
        private int currentResolution_height = 480;
        private bool fullScreen = true;
        private bool vSync = true;
        private int qualityLevelIndex = 6;
        //private string inputActionRebindings;

        public float dfaultMasterVolume = 80f;
        public float dfaultMusicVolume = 70f;
        public float dfaultSfxVolume = 80f;
        public float dfaultSensitivity = 10f;
        public int dfaultCurrentResolutionIndex = 0;
        public int dfaultCurrentResolution_width = 1920;
        public int dfaultCurrentResolution_height = 1080;
        public bool dfaultFullScreen = true;
        public bool dfaultVsync = true;
        public int dfaultQualityLevelIndex = 6;
        public bool usePersitentDataPath = false;
        //public string defaultBindings;

        public float MasterVolume => masterVolume;
        public float MusicVolume => musicVolume;
        public float SfxVolume => sfxVolume;    
        public int CurrentResolutionIndex => currentResolutionIndex;
        public int CurrentResolution_width => currentResolution_width;
        public int CurrentResolution_height => currentResolution_height;
        public bool FullScreen => fullScreen;
        public bool VSync => vSync;
        public int QualityLevelIndex => qualityLevelIndex;
        //public string InputActionRebindings => inputActionRebindings;

        #endregion

        #region SetValues

        public void SetMasterVolume(float value)
        {
            masterVolume = value;
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = value;
        }

        public void SetSFXVolume(float value)
        {
            sfxVolume = value;
        }

        public void SetResolutionValues(int width, int height, int indx)
        {
            currentResolutionIndex = indx;
            currentResolution_width = width;
            currentResolution_height = height;
        }

        public void SetFullscreenStatus(bool status)
        {
            fullScreen = status;
        }

        public void SetVsyncStatus(bool status)
        {
            vSync = status;
        }

        public void SetQualityLevelIndex(int value)
        {
            qualityLevelIndex = value;
        }

        /*public void SetBindings(string bindings)
        {
            if (string.IsNullOrEmpty(bindings)) return;
            inputActionRebindings = bindings;
        }*/

        public void SetDefaultValues()
        {
            masterVolume = dfaultMasterVolume;
            musicVolume = dfaultMusicVolume;
            sfxVolume = dfaultSfxVolume;           
            currentResolutionIndex = dfaultCurrentResolutionIndex;
            currentResolution_width = dfaultCurrentResolution_width;
            currentResolution_height = dfaultCurrentResolution_height;
            fullScreen = dfaultFullScreen;
            vSync = dfaultVsync;
            qualityLevelIndex = dfaultQualityLevelIndex;

            SaveValues();
            Debug.Log("<color=cyan>Default Settings Loaded</color>");
        }

        #endregion

        #region Load and Save
        private string GetFileString()
        {
            string file = string.Empty;
            if (usePersitentDataPath)
            {
                file = Application.persistentDataPath + "\\" + "settings.cfg";
            }
            else
            {
                file = "settings.cfg";
            }

            return file;
        }

        public void SaveValues()
        {
            string file = GetFileString();
            string content = string.Empty;

            content += masterVolume.ToString() + "\n";
            content += musicVolume.ToString() + "\n";
            content += sfxVolume.ToString() + "\n";
            content += currentResolutionIndex.ToString() + "\n";
            content += currentResolution_width.ToString() + "\n";
            content += currentResolution_height.ToString() + "\n";
            content += fullScreen.ToString() + "\n";
            content += vSync.ToString() + "\n";
            content += qualityLevelIndex.ToString();

            Serialization.SaveToFileText(content, file);

            //if (string.IsNullOrEmpty(inputActionRebindings)) return;
            //Serialization.SaveText(inputActionRebindings, "rebindings.data");*/
        }

        public void LoadValues()
        {
            string file = GetFileString();

            if (Serialization.FileExistenceCheck(file))
            {
                List<string> content = Serialization.LoadFromFileTextByLine(file);

                masterVolume = float.Parse(content[0]);
                musicVolume = float.Parse(content[1]);
                sfxVolume = float.Parse(content[2]);
                currentResolutionIndex = int.Parse(content[3]);
                currentResolution_width = int.Parse(content[4]);
                currentResolution_height = int.Parse(content[5]);
                fullScreen = bool.Parse(content[6]);
                vSync = bool.Parse(content[7]);
                qualityLevelIndex = int.Parse(content[8]);

                Debug.Log("<color=cyan>Saved Settings Loaded</color>");
            }
            else
            {
                SetDefaultValues();
            }

            /*if (!SaveAndLoad.FileExistenceCheck("rebindings.data"))
            {
                inputActionRebindings = "";
            }
            else
            {
                inputActionRebindings = SaveAndLoad.LoadFromFile("rebindings.data");
            }*/
        }

        #endregion
    }
}