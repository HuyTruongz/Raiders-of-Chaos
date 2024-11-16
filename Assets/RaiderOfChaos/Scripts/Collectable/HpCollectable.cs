using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class HpCollectable : Collectable
    {
        protected override void TriggerCore()
        {
            GameManager.Ins.AddHp(m_bonus);
        }
    }
}
