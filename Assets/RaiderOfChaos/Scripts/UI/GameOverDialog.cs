using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hyhy.RaidersOfChaos
{
    public class GameOverDialog : Dialog
    {
        public Text gameplayTimeTxt;
        public Text bestTimeTxt;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
            if (gameplayTimeTxt)
            {
                gameplayTimeTxt.text = Helper.TimeConvert(GameManager.Ins.GplayTimeCounting);
            }

            if (bestTimeTxt)
            {
                float bestTime = GameData.Ins.GetLevelScore(GameData.Ins.curLevelId);
                bestTimeTxt.text = Helper.TimeConvert(bestTime);
            }
        }

        public void Replay()
        {
            Close();
            GameManager.Ins.Replay();
        }

        public void BackHome()
        {
            Time.timeScale = 1f;
            SceneController.Ins.LoanScene(GameScene.MainMenu.ToString());
        }
    }
}
