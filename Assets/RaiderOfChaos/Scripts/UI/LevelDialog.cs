using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hyhy.RaidersOfChaos
{
    public class LevelDialog : Dialog
    {
        public Transform gridRoot;
        public LevelItemUI itemUIPb;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
            UpdateUI();
        }

        private void UpdateUI()
        {
            var leves = LevelManager.Ins.levels;

            if (leves == null || leves.Length <= 0 || !gridRoot || !itemUIPb) return;

            Helper.ClearChilds(gridRoot);

            for (int i = 0; i < leves.Length; i++)
            {
                int levelId = i;
                var level = leves[i];
                if (level == null) continue;

                var itemUIClone = Instantiate(itemUIPb, Vector3.zero, Quaternion.identity);
                itemUIClone.transform.SetParent(gridRoot);
                itemUIClone.transform.localScale = Vector3.one;
                itemUIClone.transform.localPosition = Vector3.zero;
                itemUIClone.UpdateUI(level, levelId);

                if (itemUIClone.btnComp)
                {
                    itemUIClone.btnComp.onClick.RemoveAllListeners();
                    itemUIClone.btnComp.onClick.AddListener(() => ItemEvent(level, levelId));
                }
            }

        }

        private void ItemEvent(LevelItem level, int levelId)
        {
            if (level == null) return;

            bool isUnlocked = GameData.Ins.IsLevelUnlocked(levelId);

            if (isUnlocked)
            {
                GameData.Ins.curLevelId = levelId;
                LevelManager.Ins.CurlevelId = levelId;
                GameData.Ins.SaveData();
                Close();
                SceneController.Ins.LoandGamePlay();
            }
            else
            {
                Debug.Log("Level not Unlock");
            }
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
