using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hyhy.RaidersOfChaos
{
    public class LevelCompletedDialog : Dialog
    {
        public Image[] stars;
        public Text gamePlayTimeTxt;
        public Text bonusTxt;
        public Sprite activeStar;
        public Sprite deactiveStar;

        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if (stars == null || stars.Length <= 0) return;

            for (int i = 0; i < stars.Length; i++)
            {
                var star = stars[i];
                if (star == null) continue;
                star.sprite = deactiveStar;
            }

            for (int i = 0; i < GameManager.Ins.Stars; i++)
            {
                var star = stars[i];
                if (star == null) continue;
                star.sprite = activeStar;
            }

            if (gamePlayTimeTxt)
            {
                gamePlayTimeTxt.text = Helper.TimeConvert(GameManager.Ins.GplayTimeCounting);
            }

            if (bonusTxt)
            {
                bonusTxt.text = GameManager.Ins.MissionCoinBouns.ToString();
            }
        }

        public void BackHome()
        {
            Time.timeScale = 1f;
            SceneController.Ins.LoanScene(GameScene.MainMenu.ToString());
        }

        public void NextLevel()
        {
            LevelItem[] levels = LevelManager.Ins.levels;

            if (levels == null || levels.Length <= 0) return;

            if(GameData.Ins.curLevelId >= LevelManager.Ins.levels.Length - 1)
            {
                SceneController.Ins.LoanScene(GameScene.MainMenu.ToString());
            }
            else
            {
                SceneController.Ins.LoandGamePlay();
            }
        }
    }
}
