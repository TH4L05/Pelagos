/// <author>Thoams Krahl</author>

using UnityEngine;
using UnityEngine.InputSystem;
using System;
using PelagosProject.User;
using PelagosProject.User.Movement;

namespace PelagosProject.Quests
{
    public class QuestGoalPlayerInput : QuestGoal
    {

        #region SerializedFields

        [Header("InputGoalSettings"), Space(5f)]
        [SerializeField, Range(0.0f, 2.0f)] private float inputRepeatCheckTime = 0.5f;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private int powerValueIndex; 

        #endregion

        public override void Setup()
        {
            base.Setup();
        }

        public override void Activate()
        {
            base.Activate();
            InvokeRepeating("CheckInput", 0f, inputRepeatCheckTime);
        }

        public override void Destroy()
        {
            base.Destroy();
            CancelInvoke("CheckInput");
        }


        private void CheckInput()
        {
            float[] powerValues = playerMovement.GetCurrentPowerValues();
            currentAmount = (int)powerValues[powerValueIndex];
            UpdateAmount();
        }

        public override void UpdateAmount()
        {
            base.UpdateAmount();
            if (isActive && currentAmount >= requiredAmount)
            {
                CancelInvoke("CheckInput");
            }
        }
    }
}




