using hyhy.SPM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class FireProjectile : UltimateController
    {
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string projectilePool;
        public float speed;
        public Transform[] firePoints;

        public override void DealDamage()
        {
            for (int i = 0; i < firePoints.Length; i++)
            {
                var point = firePoints[i];
                if (point == null) continue;
                GameObject p = PoolersManager.Ins.Spawn(PoolerTarget.NONE, projectilePool, point.position, Quaternion.identity);
                if (!p || !m_owner) return;

                Projectile pComp = p.GetComponent<Projectile>();

                if (!pComp) return;

                pComp.damageTo = m_owner.damageTo;
                if (speed > 0)
                {
                    pComp.speed = speed;
                }

                pComp.damage = m_owner.CurDmg;
                pComp.owenr = m_owner;

                if (point.position.x < m_owner.transform.position.x)
                {
                    if (p.transform.localScale.x > 0)
                    {
                        p.transform.localScale = new Vector3(
                            p.transform.localScale.x * -1,
                            p.transform.localScale.y,
                            p.transform.localScale.z);
                    }

                    if (pComp && pComp.speed > 0)
                    {
                        pComp.speed *= -1;
                    }
                }
                else
                {
                    if (p.transform.localScale.x < 0)
                    {
                        p.transform.localScale = new Vector3(
                            p.transform.localScale.x * -1,
                            p.transform.localScale.y,
                            p.transform.localScale.z);
                    }

                    if (pComp && pComp.speed < 0)
                    {
                        pComp.speed *= -1;
                    }
                }
            }
        }
    }
}
