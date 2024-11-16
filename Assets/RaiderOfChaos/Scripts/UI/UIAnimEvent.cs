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
            if (GUIManager.Ins.completedDialog)
            {
                GUIManager.Ins.completedDialog.Show(true);
            }
        }

        public void ShowGameOverDialog()
        {
            if (GUIManager.Ins.gameoverDialog)
            {
                GUIManager.Ins.gameoverDialog.Show(true);
            }
        }
    }
}
