using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class UltimateController : MonoBehaviour, IDamageCreater
    {
        [Range(0f, 1f)]
        public float rate;

        protected Actor m_owner;

        public Actor Owner { get => m_owner; set => m_owner = value; }

        public virtual void DealDamage()
        {

        }
    }
}