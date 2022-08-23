/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PelagosProject.User;
using PelagosProject.User.Input;
using PelagosProject.UI;
using PelagosProject.UI.Menu;
using PelagosProject.UI.Menu.Ingame;
using PelagosProject.UI.HUD;
using PelagosProject.Save.Profile;

namespace PelagosProject
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private IngameMenu ingameMenu;
        [SerializeField] private Hud hud;
        [SerializeField] private PhotoMode photoMode;
        [SerializeField] private PlayerProfileSlot playerProfileSlot;
        [SerializeField] private Options options;
        [SerializeField] private bool mainMenu = false;
        [SerializeField] private Player player;
        [SerializeField] private EventSystem eventSystem;
        private bool paused;
     
        public static Game Instance;
        public PlayerInput Input => input;
        public Hud Hud => hud;
        public PlayerProfileSlot PlayerProfileSlot => playerProfileSlot;
        public Player Player => player;
        public EventSystem EventSystem => eventSystem;

        private void Awake()
        {
            Instance = this;
            Time.timeScale = 1f;
            Application.targetFrameRate = 60;                      
        }

        private void Start()
        {
            if (options) options.Setup();
            if (playerProfileSlot == null) return;

            LoadProfile();
            if (playerProfileSlot.playerProfile == null) return;
            if(!string.IsNullOrEmpty(playerProfileSlot.playerProfile.name)) ScannableLibary.Instance.SetScannablesUnlockStatus(playerProfileSlot.playerProfile.scannablesUnlockStatus);

            //-> Unused <-
            //if (mainMenu) return;
            //playerProfileSlot.SetProfilePlayedStatus();
            //playerProfileSlot.SetLastPlayedLevel(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDestroy()
        {
            SaveProfile();          
        }

        private void Update()
        {
            TogglePauseMenuOnInput();
        }

        private void TogglePauseMenuOnInput()
        {
            if (mainMenu) return;
            if (input.IngameMenuIsActive)
            {
                if (paused)
                {
                    paused = false;
                    ingameMenu.ToggleMenu();

                }
                else
                {
                    paused = true;
                    ingameMenu.ToggleMenu();
                }
            }
            input.IngameMenuIsActive = false;
        }

        public void SaveProfile()
        {
            if (playerProfileSlot == null) return;
            playerProfileSlot.UpdateUnlockStatus();
            playerProfileSlot.SetLastPlayedLevel(SceneManager.GetActiveScene().buildIndex);
            playerProfileSlot.SavePlayerProfile();
        }

        public void LoadProfile()
        {
            if (playerProfileSlot == null) return;
            playerProfileSlot.LoadPlayerProfile();
        }
    }
}

