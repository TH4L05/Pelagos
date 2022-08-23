/// <author>Thoams Krahl</author>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PelagosProject.Quests
{
    public class QuestSystem : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private List<Quest> quests = new List<Quest>();
        [SerializeField] private  bool activateFirstQuestOnStart;
        [SerializeField] [Range(0.0f, 5.0f)] private float startDelayTime = 1.5f;
        [SerializeField] [Range(0.0f, 5.0f)] private float nextQuestStartDelayTime = 1.0f;

        #endregion

        #region PrivateFields

        private int activeQuestIndex;

        #endregion;

        #region UnityFunctions

        private void Awake()
        {        
        }

        private void Start()
        {
            Quest.QuestIsComplete += AQuestIsComplete;
            foreach (var quest in quests)
            {
                quest.Setup();
            }

            activeQuestIndex = 0;
            if (!activateFirstQuestOnStart) return;
            ActivateQuest(activeQuestIndex);
        }
        
        public void OnDestroy()
        {
            Quest.QuestIsComplete -= AQuestIsComplete;
            foreach (var quest in quests)
            {
                quest.DestroyStart();
            }
        }

        #endregion

        #region ActivateAndDeactivate
        
        public void ActivateQuest(int index)
        {
            if (index < 0 || index >= quests.Count)
            {
                Debug.LogError("Non possible Quest Index Value");
                return;
            }


            if (quests[index].IsComplete)
            {
                Debug.LogError("Quest is already complete");
                return;
            }
            StartCoroutine(ActivateAQuest(index));
        }

        public void ActivateQuest(string questName)
        {
            int idx = 0;
            foreach (var quest in quests)
            {
                idx++;
                if (quest.QuestName == questName)
                {
                    ActivateQuest(idx);
                    return;
                }
            }
        }
        
        IEnumerator ActivateAQuest(int index)
        {
            yield return new WaitForSeconds(startDelayTime);          
            quests[index].SetActive();
            startDelayTime = nextQuestStartDelayTime;
        }

        public void DeactivateAllActiveQuests()
        {
            foreach (var quest in quests)
            {
                if (!quest.IsActive) continue;
                quest.Deactivate();
            }
        }
        
        #endregion

        public void SetNextQuestStartDelayTime(float time)
        {
            nextQuestStartDelayTime = time;
        }

        public void AQuestIsComplete(Quest quest)
        {
            activeQuestIndex++;

            if (activeQuestIndex > quests.Count) return;
            if (!quest.ActivateNextQuestWhenComplete) return;
            ActivateQuest(activeQuestIndex);
        }
    }
}

