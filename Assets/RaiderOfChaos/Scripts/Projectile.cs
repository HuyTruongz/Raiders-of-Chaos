using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hyhy.SPM;
using System;

namespace hyhy.RaidersOfChaos
{
    public class Projectile : MonoBehaviour/*, IDamageCreater*/
    {
        [Header("Base Setting:")]
        public GameTag damageTo;
        public float speed;
        public float damage;

        [PoolerKeys(target = PoolerTarget.NONE)]
        public string bodyHitPool;

        [HideInInspector]
        public Actor owner;

        private Vector2 m_lastPos;
        private RaycastHit2D m_hit;

        private void OnEnable()
        {
            RefereshLastPos();
        }

        private void RefereshLastPos()
        {
           
        }
    }

}