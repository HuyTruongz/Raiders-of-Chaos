using hyhy.SPM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class CollectableManager : Singleton<CollectableManager>
    {
        public CollectableItem[] items;

        public void Spawn(Vector3 pos)
        {
            if (items == null || items.Length <= 0) return;
            var rateCheking = Random.Range(0f, 1f);

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (item == null) continue;

                if(item.spawnRate >= rateCheking)
                {
                    for (int j = 0; j < item.amount; j++)
                    {
                        GameObject c = PoolersManager.Ins.Spawn(PoolerTarget.NONE,item.collectablePool,pos,Quaternion.identity);
                        if (c)
                        {
                            Collectable cComp = c.GetComponent<Collectable>();
                            if (cComp)
                            {
                                cComp.Init();
                            }
                        }
                    }
                }
            }
        }
    }
}
