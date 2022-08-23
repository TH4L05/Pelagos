/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PelagosProject.User;

namespace PelagosProject.UI.HUD
{
    public class ShipInformationsDisplay : MonoBehaviour
    {
        private const float MinAngle = 118.0f;
        private const float MaxAngle = -118.0f;

        #region SerializedFields

        [SerializeField] private Transform speedTachoNeedle;
        [SerializeField] private TextMeshProUGUI speedValueField;
        [SerializeField] private string speedValueSuffix;


        [SerializeField] private Transform depthTachoNeedle;
        [SerializeField] private TextMeshProUGUI depthValueField;
        [SerializeField] private string depthValueSuffix;
        [SerializeField] private float maxDepthValue = -6000.0f;


        [SerializeField] private TextMeshProUGUI pressureValueField;
        [SerializeField] private string pressureValueSuffix;
        [SerializeField] private Image pressureBar;
        [SerializeField] private float maxPressureValue = 700.0f;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Player.ShipInformationsChanged += UpdateInfoDisplay;
        }

        private void OnDestroy()
        {
            Player.ShipInformationsChanged -= UpdateInfoDisplay;
        }

        #endregion

        private void UpdateInfoDisplay(float[] infoValues)
        {
            depthValueField.text = infoValues[0].ToString("000.0") + " " + depthValueSuffix;
            pressureValueField.text = infoValues[1].ToString("000.0") + " " + pressureValueSuffix;
            speedValueField.text = infoValues[2].ToString("000.0") + " " + speedValueSuffix;

            UpdateDepthTachoNeedleRotation(infoValues[0], maxDepthValue);
            UpdateSpeedTachoNeedleRotation(infoValues[2], infoValues[3]);
            UpdatePressureBarFill(infoValues[1], maxPressureValue);
        }

        private void UpdateDepthTachoNeedleRotation(float depth, float depthMax)
        {
            float totalAngle = MinAngle - MaxAngle;
            float normalizedDepth = depth / depthMax;
            float angle = MinAngle - normalizedDepth * totalAngle;
            depthTachoNeedle.eulerAngles = new Vector3(0f, 0f, angle);

        }

        private void UpdateSpeedTachoNeedleRotation(float speed, float speedMax)
        {
            float totalAngle = MinAngle - MaxAngle;
            float normalizedSpeed = speed / speedMax;

            float angle = MinAngle - normalizedSpeed * totalAngle;
            speedTachoNeedle.eulerAngles = new Vector3(0f, 0f, angle);

        }

        private void UpdatePressureBarFill(float pressure, float pressureMax)
        {
            float fillAmount = pressure / pressureMax;
            pressureBar.fillAmount = fillAmount;

        }
    }
}

