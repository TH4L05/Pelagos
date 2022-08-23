/// <author>Thoams Krahl</author>

using System;
using UnityEngine;
using FMODUnity;

namespace PelagosProject.Quests
{
    public class QuestGoal : MonoBehaviour
    {
        #region Events

        public Action QuestGoalGoalReached;
        public Action<QuestGoal> QuestGoalGoalUpdate;
        public Action<EventReference> PlayAudioEvent;

        #endregion

        #region SerializedFields

        [Header("Settings"), Space(5f)]
        [SerializeField] protected string goalName;
        [SerializeField] protected string description;
        [SerializeField] protected int requiredAmount = 1;
        [SerializeField] protected bool playAudio = true;
        [SerializeField] protected EventReference audioStartEvent;
        [SerializeField] protected EventReference audioEndEvent;
        
        [Header("DEV"), Space(5f)]
        //TODO: make private after Testing
        [SerializeField] protected bool isActive;
        [SerializeField] protected bool isDone;
        [SerializeField] protected int currentAmount;

        #endregion

        #region PrivateFields



        #endregion

        #region PublicFields

        public string Description => description;
        public int RequiredAmount => requiredAmount;
        public int CurrentAmount => currentAmount;
        public bool IsActive => isActive;
        public bool IsDone => isDone;

        #endregion

        public virtual void Setup()
        { 
        }

        public virtual void Destroy()
        {
            isActive = false;
            isDone = true;
        }

        public virtual void UpdateAmount()
        {
            if (isActive)
            {
                currentAmount++;
                QuestGoalGoalUpdate?.Invoke(this);
                GoalComplete();
            }
        }

        public virtual void GoalComplete()
        {
            if (isDone) return;

            if (currentAmount >= requiredAmount)
            {
                isDone = true;
                isActive = false;
                QuestGoalGoalReached?.Invoke();
                if(!audioEndEvent.IsNull && playAudio) PlayAudioEvent?.Invoke(audioEndEvent);
            }
        }

        public virtual void Activate()
        {           
            isActive = true;
            isDone = false;
            if (!audioEndEvent.IsNull && playAudio) PlayAudioEvent?.Invoke(audioStartEvent);
        }

        public void ResetQuestGoal()
        {
            isActive = false;
            isDone = false;
        }
    }
}

