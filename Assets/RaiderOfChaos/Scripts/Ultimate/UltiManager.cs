using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace hyhy.RaidersOfChaos
{
    public class UltiManager : MonoBehaviour
    {
        public UltimateController[] ultiCtrl;

        private Actor m_owner;

        public Actor Owner { get => m_owner; set => m_owner = value; }

        public void UltiTrigger()
        {
            float rateChecking = Random.Range(0f,1f);
            var finder = ultiCtrl.Where(u => u.rate >= rateChecking);

            if (finder == null) return;

            var rs = finder.ToArray();

            if(rs == null || rs.Length <= 0) return;

            int randIdx = Random.Range(0,rs.Length);

            var ultCtr = rs[randIdx];

            if(!ultCtr) return;

            ultCtr.Owner = m_owner;
            ultCtr.DealDamage();
            CamShake.ins.ShakeTrigger(0.2f,0.3f);
        }
    }
}
