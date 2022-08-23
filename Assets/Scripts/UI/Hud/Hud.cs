/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using PelagosProject.Audio;
using PelagosProject.Interactables;
using PelagosProject.Quests;
using PelagosProject.Puzzles;

namespace PelagosProject.UI.HUD
{
    public class Hud : MonoBehaviour
    {
        #region Fields

        [Header("CrossHair"), Space(2f)]
        [SerializeField] private Image crosshair;
        [SerializeField] private Sprite crosshairDefaultSprite;
        [SerializeField] private Image crosshair2;
        [SerializeField] private Color crosshairColorDefault = Color.white;
        [SerializeField] private Color crosshairColorPuzzle = new Color(1f,1f,1f,0.35f);

        [Header("InfoFields"), Space(2f)]
        [SerializeField] private TextMeshProUGUI creatureNameField;
        [SerializeField] private Animator anim;
        [SerializeField] private TextMeshProUGUI informationTextField;
        [SerializeField] private PlayableDirector informationText;

        [Header("Audio"), Space(2f)]
        [SerializeField] private AudioEventList audioEvents;

        [Header("Scan"), Space(2f)]
        [SerializeField] private Image scanFillbarImage;

        [Header("Quests"), Space(2f)]
        [SerializeField] private TextMeshProUGUI questTitel;
        [SerializeField] private TextMeshProUGUI questGoalDescription;
        [SerializeField] private TextMeshProUGUI questGoalAmount;


        private float currentScanTime;
        private bool onPuzzle;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Setup();
        }

        private void Start()
        {
            ScannableLibary.CreatureUnlocked += ShowInformationText;
        }

        private void OnDestroy()
        {
            Destroy();
        }

        #endregion

        #region SetupAndDestroy

        private void Setup()
        {
            Quest.QuestIsStarting += UpdateQuestInformationFields;
            Quest.QuestIsComplete += UpdateQuestInfoOnQuestComplete;
            Quest.QuestGoalReached += UpdateQuestInformationFields;
            Quest.QuestGoalUpdated += UpdateQuestGoalInformations;
            Scanner.ScannableIsOnFocus += ScannableOnFocus;
            Scanner.UpdateScanProgress += FillScanBar;
            Scanner.ScanIsComplete += ResetScanBar;
            Puzzle.PuzzleComplete += PuzzleFinished;
            Puzzle.PuzzleStarted += SetPuzzleSprite;

            SetDefaultCrosshair();
            creatureNameField.text = "";
            creatureNameField.gameObject.SetActive(false);
            //crosshair2.gameObject.SetActive(false);
        }

        private void Destroy()
        {
            ScannableLibary.CreatureUnlocked -= ShowInformationText;
            Scanner.ScannableIsOnFocus -= ScannableOnFocus;
            Scanner.UpdateScanProgress += FillScanBar;
            Scanner.ScanIsComplete -= ResetScanBar;
            Puzzle.PuzzleComplete -= PuzzleFinished;
            Puzzle.PuzzleStarted -= SetPuzzleSprite;
            Quest.QuestIsStarting -= UpdateQuestInformationFields;
            Quest.QuestIsComplete -= UpdateQuestInfoOnQuestComplete;
            Quest.QuestGoalReached -= UpdateQuestInformationFields;
            Quest.QuestGoalUpdated -= UpdateQuestGoalInformations;
        }

        #endregion;

        #region Scanbar

        private void FillScanBar(bool active, float currentScanProgress, float scanDuration)
        {
            if (active)
            {
                scanFillbarImage.gameObject.SetActive(true);
                if (scanFillbarImage != null) scanFillbarImage.fillAmount = currentScanProgress / scanDuration;
                return;
            }
            scanFillbarImage.gameObject.SetActive(false);

        }

        private void ResetScanBar()
        {
            if (scanFillbarImage != null) scanFillbarImage.fillAmount = 0f;
            scanFillbarImage.gameObject.SetActive(false);
        }

        #endregion

        #region InfoTextField

        private void ShowInformationText(ScannableData data)
        {
            if (data == null)
            {
                informationTextField.text = "Scannable Data is Missing";
            }
            else
            {
                informationTextField.text = $"{data.InteractableName} Unlocked";
            }
            
            informationText.Play();
            audioEvents.PlayAudioEvent("ScanComplete");
        }

        private void ShowInformationText(string text)
        {
            informationTextField.text = text;
            informationText.Play();
            audioEvents.PlayAudioEvent("ScanComplete");
        }

        #endregion

        #region Quest

        private void UpdateQuestInformationFields(Quest quest)
        {
            Debug.Log("Show Quest UI");
            if (questTitel) questTitel.gameObject.SetActive(true);
            if (questGoalDescription) questGoalDescription.gameObject.SetActive(true);
            if (questGoalAmount) questGoalAmount.gameObject.SetActive(true);

            if (questTitel) questTitel.text = quest.QuestName;

            var questGoal = quest.GetActiveQuestGoal();
            UpdateQuestGoalInformations(questGoal);
        }

        private void UpdateQuestGoalInformations(QuestGoal questGoal)
        {
            if (questGoalDescription) questGoalDescription.text = questGoal.Description;
            if (questGoalAmount) questGoalAmount.text = questGoal.CurrentAmount + "/" + questGoal.RequiredAmount;
        }

        private void UpdateQuestInfoOnQuestComplete(Quest quest)
        {
            if (questTitel) questTitel.gameObject.SetActive(false);
            if (questGoalDescription) questGoalDescription.gameObject.SetActive(false);
            if (questGoalAmount) questGoalAmount.gameObject.SetActive(false);
        }
        
        #endregion

        public void SetPuzzleSprite(Sprite puzzleSprite)
        {        
            if (crosshair != null)
            {
                crosshair.sprite = puzzleSprite;
                crosshair.color = crosshairColorPuzzle;
                crosshair.transform.localScale = new Vector3(8f, 8f, 8f);
            }
            onPuzzle = true;
        }

        public void SetDefaultCrosshair()
        {
            if (crosshair != null)
            {
                crosshair.transform.localScale = new Vector3(1f, 1f, 1f);
                crosshair.sprite = crosshairDefaultSprite;
                crosshair.color = crosshairColorDefault;
            }
            onPuzzle = false;
        }

        private void PuzzleFinished()
        {
            ShowInformationText("PuzzleFinished");
            SetDefaultCrosshair();
        }

        private void ScannableOnFocus(bool onFocus, int creatureLibaryIndex)
        {
            if (onPuzzle) return;

            anim.SetBool("onFocus", onFocus);
            ScannableData scannableData = null;
            if (creatureLibaryIndex >= 0)
            {
                scannableData = ScannableLibary.Instance.GetScannableData(creatureLibaryIndex);
            }

            if (onFocus)
            {
                if (scannableData != null && scannableData.IsUnlocked)
                {
                    creatureNameField.text = scannableData.InteractableName;
                }               
                else
                {
                    creatureNameField.text = "UNKNOWN";
                }
            }
            creatureNameField.gameObject.SetActive(onFocus);
        }

    }
}

