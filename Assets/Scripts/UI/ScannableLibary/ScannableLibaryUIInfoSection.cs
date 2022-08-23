/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PelagosProject.Interactables;

namespace PelagosProject.UI.Menu.Ingame
{

    public class ScannableLibaryUIInfoSection : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private TextMeshProUGUI scannableNameField;
        [SerializeField] private ScannableLibaryUIInfoSectionValue[] infoValues;
        //[SerializeField] private Image scannableLockedImage;
        [SerializeField] private TextMeshProUGUI unlockedInfoTextField;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        private void OnEnable()
        {
            ScannableLibaryUISlot.ShowInformations += UpdateInfo;
        }

        private void OnDisable()
        {
            ScannableLibaryUISlot.ShowInformations -= UpdateInfo;
            //scannableLockedImage.gameObject.SetActive(true);
        }

        #endregion



        private void UpdateInfo(ScannableData data)
        {
            int unlockedCount = ScannableLibary.Instance.GetUnlockedCount();
            int scannableCount = ScannableLibary.Instance.ScannableDataCount;
            if (unlockedInfoTextField) unlockedInfoTextField.text = $"{unlockedCount}/{scannableCount} Scannables unlocked";


            if (data == null)
            {

            }


            if (!data.IsUnlocked)
            {
                if (scannableNameField) scannableNameField.text = "Unkown Entry";
                foreach (var info in infoValues)
                {
                    info.infoTextValueField.text = "???";
                }
            }
            else
            {
                //scannableLockedImage.gameObject.SetActive(false);
                switch (data.ScannableType)
                {
                    case ScannableType.Invalid:
                        Debug.LogError("ERROR - Cant show info - Invalid ScannableType");
                        break;

                    case ScannableType.Creature:
                        var creatureData = data as CreatureData;
                        UpdateCreatureInfo(creatureData);                       
                        break;

                    case ScannableType.Plant:
                        var plantData = data as PlantData;
                        UpdatePlantInfo(plantData);
                        break;

                    case ScannableType.Fungus:
                        break;

                    default:
                        break;
                }
            }
        }

        private void UpdateCreatureInfo(CreatureData creatureData)
        {
            if (scannableNameField) scannableNameField.text = creatureData.InteractableName;

            infoValues[0].SetTextValue(nameof(creatureData.Species), creatureData.Species.ToString());

            string habitat = string.Empty;
            habitat += creatureData.Habitat1.ToString();

            if (creatureData.Habitat2 != Habitat.None && creatureData.Habitat2 != Habitat.Invalid)
            {
                habitat += ", ";
                habitat += creatureData.Habitat2.ToString();
            }

            infoValues[1].SetTextValue("", habitat);
            infoValues[2].SetTextValue(nameof(creatureData.Size), creatureData.Size.ToString("0 cm"));

            float weightValue = creatureData.Weight;
            string weight = string.Empty;

            if (weightValue > 9999)
            {
                weight = (weightValue / 1000).ToString("0 kg");
            }
            else
            {
                weight = weightValue.ToString("0 g");
            }

            infoValues[3].SetTextValue(nameof(creatureData.Weight), weight);
            infoValues[4].SetTextValue("", creatureData.Description);
        }

        private void UpdatePlantInfo(PlantData plantData)
        {
            if (scannableNameField) scannableNameField.text = plantData.InteractableName;

            string habitat = string.Empty;
            habitat += plantData.Habitat1.ToString();

            if (plantData.Habitat2 != Habitat.None && plantData.Habitat2 != Habitat.Invalid)
            {
                habitat += ", ";
                habitat += plantData.Habitat2.ToString();
            }

            infoValues[1].SetTextValue("", habitat);
            infoValues[2].SetTextValue(nameof(plantData.Size), plantData.Size.ToString("0 cm"));
            infoValues[4].SetTextValue("", plantData.Description);
        }
    }

    #region LibaryUISectionValue

    [System.Serializable]
    internal class ScannableLibaryUIInfoSectionValue
    {
        public TextMeshProUGUI infoTextNameField;
        public TextMeshProUGUI infoTextValueField;

        public void SetTextValue(string name, string value)
        {
            if(!string.IsNullOrEmpty(name)) infoTextNameField.text = name;
            if(!string.IsNullOrEmpty(value)) infoTextValueField.text = value;
        }
    }

    #endregion
}

