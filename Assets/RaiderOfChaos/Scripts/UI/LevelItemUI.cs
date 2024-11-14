using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hyhy.RaidersOfChaos
{
    public class LevelItemUI : MonoBehaviour
    {
        public Text levelTxt;
        public Image lockIcon;
        public Image[] starImgs;
        public Button btnComp;
        public Sprite activeStar;
        public Sprite deactiveStar;

        public void UpdateUI(LevelItem levelItem, int levelId)
        {
            if (levelItem == null) return;
            bool isUnlocked = GameData.Ins.IsLevelUnlocked(levelId);
            int stars = GameData.Ins.GetLevelStars(levelId);

            if (starImgs != null && starImgs.Length > 0)
            {
                for (int i = 0; i < starImgs.Length; i++)
                {
                    var starImg = starImgs[i];
                    if (starImg == null) continue;
                    starImg.sprite = deactiveStar;
                }

                for (int i = 0; i < stars; i++)
                {
                    var starImg = starImgs[i];
                    if (starImg == null) continue;
                    starImg.sprite = activeStar;
                }
            }

            if (levelTxt)
            {
                levelTxt.gameObject.SetActive(isUnlocked);
                levelTxt.text = (levelId + 1).ToString();
            }

            if (lockIcon)
            {
                lockIcon.gameObject.SetActive(!isUnlocked);
            }
        }
    }
}
