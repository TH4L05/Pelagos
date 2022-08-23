/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace PelagosProject.Quests
{  
    public class QuestGoalCheckInput : QuestGoal
    {
        #region SerializedFields

        [Header("InputGoalSettings"), Space(5f)]
        [SerializeField, Range(0.0f, 2.0f)] private float inputRepeatCheckTime = 0.5f;
        [SerializeField] private float goalValue = -1;
        [SerializeField] private bool goalBool;
        [SerializeField] private Vector3 goalVector = Vector3.zero;
        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private bool CheckIfPerformed;

        [Header("TEST"), Space(5f)]
        [SerializeField] private bool currentBoolValue;
        [SerializeField] private float currentFloatValue;
        [SerializeField] private Vector3 currentVectorValue;

        #endregion

        #region PrivateFields

        private InputAction inputAction;      
        private Type goalType;
        private object currentValue;

        #endregion

        public override void Activate()
        {
            base.Activate();
            InvokeRepeating("CheckInput", 0f, inputRepeatCheckTime);
            inputAction = inputActionReference.action;
            if (inputAction != null) inputAction.performed += InputActionPerformed;
            if (inputAction != null) inputAction.Enable();
        }

        public override void Destroy()
        {
            base.Destroy();
            if (inputAction != null) inputAction.performed -= InputActionPerformed;
            if (inputAction != null) inputAction.Disable();
            CancelInvoke("CheckInput");
        }

        private void CheckInput()
        {
            if (inputActionReference == null) return;
            if (inputAction == null) return;
            if (currentValue == null) return;

            goalType = currentValue.GetType();
            CheckGoalValue(goalType);
            
            /*if (checkValue)
            {              
                if (currentFloatValue == goalValue)
                {
                    CancelInvoke("CheckInput");
                    UpdateAmount();
                }
            }*/
        }

        private void CheckGoalValue(Type type)
        {
            if (goalType == typeof(bool))
            {
                bool value = (bool)currentValue;
                currentBoolValue = value;

                if (currentBoolValue == goalBool)
                {
                    UpdateAmount();
                }
            }
            else if (goalType == typeof(float))
            {
                float value = (float)currentValue;
                currentFloatValue = value;
                if (goalValue > 0 && currentFloatValue >= goalValue)
                {
                    UpdateAmount();
                }
                else if(goalValue < 0 && currentFloatValue <= goalValue)
                {
                    UpdateAmount();
                }

            }
            else if (goalType == typeof(Vector2))
            {
                Vector3 value = (Vector2)currentValue;
                currentVectorValue = value;
                if (currentVectorValue == goalVector && goalVector != Vector3.zero)
                {                    
                    UpdateAmount();
                }
            }
        }

        private void InputActionPerformed(InputAction.CallbackContext callbackContext)
        {
            if (CheckIfPerformed)
            {
                currentValue = false;
                return;
            }
            currentValue = callbackContext.ReadValueAsObject();   
        }

        public override void UpdateAmount()
        {
            base.UpdateAmount();
            if (IsActive && CurrentAmount >= RequiredAmount)
            {
                CancelInvoke("CheckInput");
            }
        }
    }
}

