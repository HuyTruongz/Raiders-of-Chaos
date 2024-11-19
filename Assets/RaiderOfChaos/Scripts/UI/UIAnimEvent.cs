using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class UIAnimEvent : MonoBehaviour
    {
       

        public void Deactive()
        {
            gameObject.SetActive(false);
        }

        public void ShowCompletDialog()
        {
            AudioController.Ins.StopPlayMusic();
            if (GUIManager.Ins.completedDialog)
            {
                GUIManager.Ins.completedDialog.Show(true);
            }
            AudioController.Ins.PlayMusic(AudioController.Ins.completed,false);
        }

        public void ShowGameOverDialog()
        {
            AudioController.Ins.StopPlayMusic();
            if (GUIManager.Ins.gameoverDialog)
            {
                GUIManager.Ins.gameoverDialog.Show(true);
            }
            AudioController.Ins.PlayMusic(AudioController.Ins.fail,false);
        }
    }
}
