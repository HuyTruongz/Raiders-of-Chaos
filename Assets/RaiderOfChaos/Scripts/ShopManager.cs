using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class ShopManager : Singleton<ShopManager>
    {
        public ShopItem[] items;

        public void Init()
        {
            if (items == null || items.Length <= 0) return;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (item == null) continue;

                if(i == 0)
                {
                    GameData.Ins.UpdatePlayerUnlockeds(i, true);
                    GameData.Ins.curLevelId = i;
                }
                else
                {
                    GameData.Ins.UpdatePlayerUnlockeds(i, false);
                }
                GameData.Ins.UpdatePlayerStats(i, item.heroBb.stat.ToJson());
            }
            GameData.Ins.SaveData();
        }
    }
}
