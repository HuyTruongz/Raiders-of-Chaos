using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class GameData : SingleTon<GameData>
    {
        public int coin;
        public int curLevelId;
        public int curPlayerId;
        public float musicVol;
        public float soundVol;
        public List<bool> levelUnlockeds;
        public List<bool> levelPasseds;
        public List<bool> playerUnlockeds;
        public List<int> levelStars;
        public List<string> playerStats;
        public List<float> completedTimes;

        public override void Awake()
        {
            base.Awake();

            levelUnlockeds = new List<bool>();
            levelPasseds = new List<bool>();
            playerUnlockeds = new List<bool>();
            levelStars = new List<int>();
            playerStats = new List<string>();
            completedTimes = new List<float>();
        }

        private void HandleData(string data)
        {
            if (string.IsNullOrEmpty(data)) return;

            JsonUtility.FromJsonOverwrite(data, this);
        }

        public void SaveData()
        {
            Pref.GameData = JsonUtility.ToJson(this);
        }

        public void LoadData()
        {
            HandleData(Pref.GameData);
        }

        private T GetValue<T>(List<T> dataList, int idx)
        {
            if (dataList == null || dataList.Count == 0 || dataList[idx] == null) return default;
            return dataList[idx];
        }

        private void UpdateValue<T>(ref List<T> dataList, int idx, T value)
        {
            if (dataList == null) return;

            if (dataList.Count <= 0 || (dataList.Count > 0 && idx >= dataList.Count))
            {
                dataList.Add(value);
            }
            else
            {
                dataList[idx] = value;
            }
        }

        public bool GetLevelUnlocked(int id)
        {
            return GetValue<bool>(levelUnlockeds, id);
        }

        public void UpdateLevelUnlocked(int id,bool isOnlocked)
        {
            UpdateValue<bool>(ref levelUnlockeds, id, isOnlocked);
        }

        public bool GetLevelPasseds(int id)
        {
            return GetValue<bool>(levelPasseds, id);
        }

        public void UpdateLevelPasseds(int id, bool isOnlocked)
        {
            UpdateValue<bool>(ref levelPasseds, id, isOnlocked);
        }

        public bool GetPlayerUnlockeds(int id)
        {
            return GetValue<bool>(playerUnlockeds, id);
        }

        public void UpdatePlayerUnlockeds(int id, bool isOnlocked)
        {
            UpdateValue<bool>(ref playerUnlockeds, id, isOnlocked);
        }

        public int GetLevelStars(int id)
        {
            return GetValue<int>(levelStars, id);
        }

        public void UpdatelevelStars(int id, int stars)
        {
            UpdateValue<int>(ref levelStars, id, stars);
        }

        public string GetPlayerStats(int id)
        {
            return GetValue<string>(playerStats, id);
        }

        public void UpdatePlayerStats(int id, string stat)
        {
            UpdateValue<string>(ref playerStats, id, stat);
        }

        public float GetCompletedTimes(int id)
        {
            return GetValue<float>(completedTimes, id);
        }

        public void UpdateCompletedTimesNoneCheck(int id,float time)
        {
            UpdateValue<float>(ref completedTimes, id, time);
        }

        public void UpdateCompletedTimes(int id, float time)
        {
            float oldTime = GetValue<float>(completedTimes, id);

            if(time < oldTime || oldTime == 0)
            {
                UpdateCompletedTimesNoneCheck(id, time);
            }
        }

        private bool IsItemUnlocked(List<bool> dataList, int idx)
        {
            if(dataList.Count <= 0 || dataList == null) return false;

            return dataList[idx];
        }

        public bool IsLevelUnlocked(int id)
        {
            return IsItemUnlocked(levelUnlockeds, id);
        }

        public bool IsLevelPassed(int id)
        {
            return IsItemUnlocked(levelPasseds, id);
        }

        public bool IsPlayerUnlocked(int id)
        {
            return IsItemUnlocked(playerUnlockeds, id);
        }
    }
}
