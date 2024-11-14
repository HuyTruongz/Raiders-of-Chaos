using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelItem[] levels;
        private int m_curlevelId;

        public int CurlevelId { get => m_curlevelId; set => m_curlevelId = value; }

        public void Init()
        {
            if (levels == null || levels.Length <= 0) return;

            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                if (level == null) continue;

                if (i == 0)
                {
                    GameData.Ins.UpdateLevelUnlocked(i, true);
                    GameData.Ins.curLevelId = i;
                }
                else
                {
                    GameData.Ins.UpdateLevelUnlocked(i, false);
                }
                GameData.Ins.UpdateLevelPasseds(i, false);
                GameData.Ins.UpdatelevelStars(i, 0);
                GameData.Ins.UpdateLevelScoreNoneCheck(i, 0);
            }
            GameData.Ins.SaveData();
        }
    }
}
