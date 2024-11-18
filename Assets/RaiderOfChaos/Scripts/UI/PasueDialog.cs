using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class PasueDialog : Dialog
    {
        public override void Show(bool isShow)
        {
            base.Show(isShow);
            Time.timeScale = 0f;
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1f;
        }

        public void Replay()
        {
            Close();
            GameManager.Ins.Replay();
        }

        public void Resume()
        {
            Close();
            Time.timeScale = 1f;
        }
    }
}
