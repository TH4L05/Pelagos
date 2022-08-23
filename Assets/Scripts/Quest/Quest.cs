/// <author>Thoams Krahl</author>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace PelagosProject.Quests
{
    public class Quest : MonoBehaviour
    {
        #region Events

        public static Action<Quest> QuestIsComplete;
        public static Action<Quest> QuestIsStarting;
        public static Action<Quest> QuestGoalReached;
        public static Action<QuestGoal> QuestGoalUpdated;

        #endregion

        #region SerializedFields

        [SerializeField] private string questName;
        //[SerializeField] private string description;
        [SerializeField] private List<QuestGoal> questGoals = new List<QuestGoal>();
        [SerializeField, Range(0.0f, 5.0f)] private float startDelayTime = 1.5f;
        [SerializeField, Range(0.0f, 5.0f)] private float nextQuestGoalStartDelay = 1.5f;
        [SerializeField] private bool activateNextQuestWhenComplete;

        #endregion

        #region PrivateFields

        private bool isActive;
        private bool isComplete;
        private int questGoalCompleteCount;
        private EventInstance audioEventInstance;
        private PLAYBACK_STATE audioPlaybackState;

        #endregion

        #region PublicFields

        public string QuestName => questName;
        public bool IsActive => isActive;
        public bool IsComplete => isComplete;
        public int GoalDoneCount => questGoalCompleteCount;
        public bool ActivateNextQuestWhenComplete => activateNextQuestWhenComplete;

        #endregion

        #region SetupAndDestroy

        public void Setup()
        {
            foreach (var questGoal in questGoals)
            {
                questGoal.Setup();
                questGoal.QuestGoalGoalReached += QuestComplete;
                questGoal.PlayAudioEvent += PlayAudioEvent;
                questGoal.QuestGoalGoalUpdate += QuestGoalUpdate;
            }
        }

        public void DestroyStart()
        {
            InvokeRepeating("CheckPlaybackState", 0, 0.25f);           
        }

        private void Destroy()
        {
            foreach (var questGoal in questGoals)
            {
                questGoal.Destroy();
                questGoal.QuestGoalGoalReached -= QuestComplete;
                questGoal.PlayAudioEvent -= PlayAudioEvent;
            }
        }

        #endregion

        #region ActivateAndDeactivate

        public void SetActive()
        {
            if (!isActive)
            {
                isActive = true;
                //QuestIsStarting?.Invoke(this);
            }
            StartCoroutine(ActivateAQuestGoal(0));
            startDelayTime = nextQuestGoalStartDelay;
        }

        public void Deactivate()
        {
            isActive = false;
            isActive = false;
            Destroy();
            StopAudioEvent();
            ReleaseAudioInstance();
        }

        IEnumerator ActivateAQuestGoal(int index)
        {
            yield return new WaitForSeconds(startDelayTime);
           
            questGoals[index].Activate();
            QuestGoalReached?.Invoke(this);
        }

        #endregion

        public void SetNextQuestGoalStartDelayTime(float time)
        {
            nextQuestGoalStartDelay = time;
        }
        
        public QuestGoal GetActiveQuestGoal()
        {
            return questGoals[questGoalCompleteCount];
        }
        
        private void QuestComplete()
        {
            if (!isActive) return;
            if (isComplete) return;
            questGoalCompleteCount++;

            if (questGoalCompleteCount >= questGoals.Count)
            {               
                isActive = false;
                QuestIsComplete?.Invoke(this);
                isActive = false;
                ReleaseAudioInstance();
                DestroyStart();
            }
            else
            {
                StartCoroutine(ActivateAQuestGoal(questGoalCompleteCount));
            }
        }

        #region Audio


        private void QuestGoalUpdate(QuestGoal questGoal)
        {
            QuestGoalUpdated?.Invoke(questGoal);
        }

        private void CheckPlaybackState()
        {
            audioEventInstance.getPlaybackState(out PLAYBACK_STATE state);
            audioPlaybackState = state;
            if (state == PLAYBACK_STATE.STARTING) return;
            if (state == PLAYBACK_STATE.PLAYING) return;

            Destroy();
            CancelInvoke("CheckPlaybackState");
        }

        public void PlayAudioEvent(EventReference eventPath)
        {
            audioEventInstance = RuntimeManager.CreateInstance(eventPath);
            audioEventInstance.start();
        }

        public void StopAudioEvent()
        {
            audioEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public void PauseAudioEvent(bool paused)
        {
            audioEventInstance.setPaused(paused);
        }

        public void ReleaseAudioInstance()
        {
            audioEventInstance.release();
        }

        #endregion
    }
}

