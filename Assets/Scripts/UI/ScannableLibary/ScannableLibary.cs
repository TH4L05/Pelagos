/// <author>Thoams Krahl</author>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PelagosProject.Interactables;

namespace PelagosProject.UI
{
    public class ScannableLibary : MonoBehaviour
    {
        #region Events

        public static Action<ScannableData> CreatureUnlocked;

        #endregion

        #region Fields

        [SerializeField] private List<ScannableData> scannableDataList = new List<ScannableData>();

        public static ScannableLibary Instance;
        public int UnlockedScannableCount {get; private set;}

        public int ScannableDataCount => scannableDataList.Count;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            UnlockedScannableCount = 0;
        }

        private void Start()
        {

        }

        private void LateUpdate()
        {
            LockAllCreatures();
            UnlockAllCreatures();
        }

        private void OnEnable()
        {
        }

        private void OnDestroy()
        {
        }

        #endregion

        public ScannableData GetScannableData(int index)
        {
            return scannableDataList[index];
        }

        public List<ScannableData> GetAllScannableData()
        {
            List<ScannableData> scannableData = new List<ScannableData>();

            foreach (var scannable in scannableDataList)
            {
                if (scannableDataList == null) continue;
                scannableData.Add(scannable);
            }
            return scannableData;
        }

        public bool CheckUnlockState(int index)
        {
            return scannableDataList[index].IsUnlocked;
        }

        public void UnlockCreature(int index)
        {
            scannableDataList[index].Unlock();
            CreatureUnlocked?.Invoke(scannableDataList[index]);
        }

        public void UnlockCreature(string name)
        {
            foreach (var scannable in scannableDataList)
            {
                if (scannable.InteractableName == name)
                {
                    scannable.Unlock();
                    CreatureUnlocked?.Invoke(scannable);
                    return;
                }
            }
        }

        public int GetUnlockedCount()
        {
            UnlockedScannableCount = 0;
            foreach (var scannable in scannableDataList)
            {
                if (!scannable.IsUnlocked) continue;
                UnlockedScannableCount++;
            }
            return UnlockedScannableCount;
        }

        public Dictionary<string, bool> GetScannablesUnlockStatus()
        {
            Dictionary<string, bool> scannablesUnlockStatus = new Dictionary<string, bool>();
            if (scannableDataList.Count == 0) return scannablesUnlockStatus;

            foreach (var scannable in scannableDataList)
            {
                scannablesUnlockStatus.Add(scannable.InteractableName, scannable.IsUnlocked);
            }

            return scannablesUnlockStatus;
        }

        public void SetScannablesUnlockStatus(Dictionary<string, bool> scannablesUnlockStatus)
        {
            if (scannablesUnlockStatus.Count == 0) return;

            foreach (var item in scannablesUnlockStatus)
            {
                foreach (var scannable in scannableDataList)
                {
                    if (scannable.InteractableName == item.Key  && item.Value == true)
                    {
                        scannable.Unlock();
                        UnlockedScannableCount++;
                        break;                       
                    }
                }
            }
        }

        #region Dev

        private void LockAllCreatures()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                foreach (var scannableData in scannableDataList)
                {
                    if (scannableData == null) continue;
                    scannableData.Lock();
                }
            }
        }

        private void UnlockAllCreatures()
        {
            if (Keyboard.current.f2Key.wasPressedThisFrame)
            {
                foreach (var scannableData in scannableDataList)
                {
                    if (scannableData == null) continue;
                    scannableData.Unlock();
                }
            }
        }

        #endregion
    }
}

