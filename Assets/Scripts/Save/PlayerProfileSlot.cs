/// <author>Thoams Krahl</author>

using UnityEngine;
using PelagosProject.UI;

namespace PelagosProject.Save.Profile
{
    public class PlayerProfileSlot : MonoBehaviour
    {
        public PlayerProfile playerProfile;
        [SerializeField] private string savegameFolderName = "Savegame";
        private bool usePersistentDataPath = true;
        private string saveFileSuffix = ".save";

        public void CreatePlayerProfile()
        {
            playerProfile = new PlayerProfile("player0");
            string path = savegameFolderName + "\\";
            string file = path + playerProfile.name + saveFileSuffix;
            if (usePersistentDataPath)
            {
                Serialization.CheckDirectory(Application.persistentDataPath + "\\" + path);
                Serialization.SaveToFile(playerProfile, Application.persistentDataPath + "\\" + file);
                return;
            }
            Serialization.CheckDirectory(path);
            Serialization.SaveToFile(playerProfile, file);         
        }

        public void DeletePlayerProfile()
        {
            bool fileExist = CheckProfileFileExistens();
            if (!fileExist) return;

            string file = savegameFolderName + "\\" + playerProfile.name + saveFileSuffix;
            playerProfile.Clear();
            playerProfile = null;

            if (usePersistentDataPath)
            {     
                Serialization.DeleteFile(Application.persistentDataPath + "\\" + file);
                return;
            }
            Serialization.DeleteFile(file);
        }

        public bool CheckProfileFileExistens()
        {
            string file = savegameFolderName + "\\" + "player0" + saveFileSuffix;
            bool fileExist;
            if (usePersistentDataPath)
            {
                fileExist = Serialization.FileExistenceCheck(Application.persistentDataPath + "\\" + file);
                return fileExist;
            }
            fileExist = Serialization.FileExistenceCheck(file);
            return fileExist;

        }

        public void LoadPlayerProfile()
        {
            bool fileExist = CheckProfileFileExistens();
            object loadedProfile = null;
            if (!fileExist) return;

            string file = savegameFolderName + "\\" + "player0" + saveFileSuffix;
            if (usePersistentDataPath)
            {
                loadedProfile = Serialization.LoadFromFile(Application.persistentDataPath + "\\" + file);
                playerProfile = (PlayerProfile)loadedProfile;
                return;
            }
            loadedProfile = Serialization.LoadFromFile(file);
            playerProfile = (PlayerProfile)loadedProfile;
        }

        public void SavePlayerProfile()
        {
            if (playerProfile == null || playerProfile.name == "") return;

            string file = savegameFolderName + "\\" + playerProfile.name + saveFileSuffix;
            if (usePersistentDataPath)
            {
                Serialization.SaveToFile(playerProfile, Application.persistentDataPath + "\\" + file);
                return;
            }
            Serialization.SaveToFile(playerProfile, file);
        }

        public void UpdateUnlockStatus()
        {
            if (playerProfile == null) return;
            playerProfile.scannablesUnlockStatus = ScannableLibary.Instance.GetScannablesUnlockStatus();
        }

        public void SetProfilePlayedStatus()
        {
            if (playerProfile == null) return;
            playerProfile.SetPlayedStatus();
        }

        public void SetLastPlayedLevel(int levelIndex)
        {
            if (playerProfile == null) return;
            playerProfile.lastPlayedLevelIndex = levelIndex;
        }
    }
}

