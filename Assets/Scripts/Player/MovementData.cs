/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.User.Movement
{
    [CreateAssetMenu(fileName ="new MovementData", menuName = "PelagosProject/Data/MovementData")]
    public class MovementData : ScriptableObject
    {
        #region SerializedFields

        [SerializeField][Range(0.1f, 25.0f)] private float powerDrag = 1.0f;
        [SerializeField][Range(0.1f, 25.0f)] private float dragMove = 1.0f;
        [SerializeField][Range(0.1f, 25.0f)] private float dragRotation = 1.0f;
        [SerializeField][Range(0.1f, 99.0f)] private float rotationSensitivityX = 15.0f;
        [SerializeField][Range(0.1f, 99.0f)] private float rotationSensitivityY = 15.0f;
        [SerializeField][Range(0.1f, 99.0f)] private float rotationSensitivityZ = 15.0f;
        [SerializeField][Range(0.1f, 99.0f)] private float rotationSensitivityGlobalY = 15.0f;
        [SerializeField][Range(1f, 999.0f)] private float powerSpeedMax = 15.0f;
        [SerializeField][Range(0.1f, 99.0f)] private float accerlation = 4.0f;
        [SerializeField][Range(0.1f, 2.0f)] private float extraMovementSpeed = 0.2f;

        #endregion

        #region PublicFields

        public float PowerDrag => powerDrag;
        public float DragMove => dragMove;
        public float DragRotation => dragRotation;
        public float RotationSensitivityX => rotationSensitivityX;
        public float RotationSensitivityY => rotationSensitivityY;
        public float RotationSensitivityZ => rotationSensitivityZ;
        public float RotationSensitivityGlobalY => rotationSensitivityGlobalY;
        public float PowerSpeedMax => powerSpeedMax;
        public float Accerlation => accerlation;
        public float ExtraMovementSpeed => extraMovementSpeed;

        #endregion
    }
}

