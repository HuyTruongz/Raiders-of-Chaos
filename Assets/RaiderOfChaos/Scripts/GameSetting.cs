using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hyhy.RaidersOfChaos
{
    public class GameSetting : MonoBehaviour
    {
        public Button mobile;
        public Image show;


        private void Update()
        {
            show.gameObject.SetActive(GameData.Ins.m_isOnMobile);
            if (mobile)
            {
                mobile.onClick.RemoveAllListeners();
                mobile.onClick.AddListener(() => ActiveMobile());
            }
        }

        private void ActiveMobile()
        {
            if (GameData.Ins.m_isOnMobile)
            {
                GameData.Ins.m_isOnMobile = false;
            }
            else
            {
                GameData.Ins.m_isOnMobile = true;
            }

            GameData.Ins.SaveData();
        }
    }
}
