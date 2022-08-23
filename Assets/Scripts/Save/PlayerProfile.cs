/// <author>Thoams Krahl</author>

using System.Collections.Generic;

namespace PelagosProject.Save.Profile
{
    [System.Serializable]
    public class PlayerProfile
    {
        public string name = string.Empty;
        public bool startedOnce;
        public int lastPlayedLevelIndex;
        public Dictionary<string, bool> scannablesUnlockStatus = new Dictionary<string, bool>();

        public PlayerProfile(string name)
        {
            this.name = name;
        }

        public void SetPlayedStatus()
        {
            startedOnce = true;
        }

        public void Clear()
        {
            name = string.Empty;
            startedOnce = false;
        }
    }
}

