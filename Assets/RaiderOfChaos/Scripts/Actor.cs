using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class Actor : MonoBehaviour
    {
        [Header("Common: ")]
        public GameTag damageTo;
        public ActorStat stat;

        [Header("Layers")]
        public GameLayer noramlLayer;
        public GameLayer invincibleLayer;
        public GameLayer deadLayer;

        [Header("Referenec: ")]
        [SerializeField]
        protected Animator m_amin;
        protected Rigidbody2D m_rb;

        public Vector3 hpBar;
        public Vector3 vfxJump;

        protected Actor m_whoHit;

        protected float m_curHp;
        protected float m_curDmg;
        protected bool m_isKnockBack;
        protected bool m_isInvincible;
        protected float m_startingGrav;
        protected bool m_isFacingLeft;
        protected float m_curSpeed;
        protected float m_dmgTaked;

        public float CurHp
        {
            get => m_curHp;
            set
            {
                m_curHp = value;
                m_curHp = Mathf.Clamp(m_curHp, 0, stat.hp);
            }
        }

        public float CurDmg
        {
            get => m_curDmg;
        }

        public bool IsFacingLeft
        {
            get => m_isFacingLeft;
        }

        public float CurSpeed
        {
            get => m_curSpeed;
        }

        public Actor WhoHit
        {
            get => m_whoHit;
            set => m_whoHit = value;
        }

        protected virtual void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
            m_startingGrav = m_rb.gravityScale;
        }

        private void LateUpdate()
        {

        }

        public virtual void Init()
        {

        }

        public virtual void TakeDamaged(float dmg, Actor whoHit)
        {
            if (m_isInvincible || m_isKnockBack) return;

            if (m_curHp > 0)
            {
                m_dmgTaked = dmg;
                m_whoHit = whoHit;

                m_curHp -= dmg;

                if (m_curHp <= 0)
                {
                    m_curHp = 0;
                    Dead();
                }
                KnockBack();
            }
        }

        private void KnockBack()
        {
            if (m_isInvincible || m_isKnockBack || !gameObject.activeInHierarchy) return;

            m_isKnockBack = true;
            StartCoroutine(StopKnockBack(stat.knockbackTime));

        }

        protected IEnumerator StopKnockBack(float time)
        {
            yield return new WaitForSeconds(time);

            m_isKnockBack = false;
            m_isInvincible = true;
            gameObject.layer = LayerMask.NameToLayer(invincibleLayer.ToString());
            StartCoroutine(StopInvincible(stat.invincibleTime));
        }

        protected IEnumerator StopInvincible(float time)
        {
            yield return new WaitForSeconds(time);
            m_isInvincible = false;
            gameObject.layer = LayerMask.NameToLayer(noramlLayer.ToString());
        }

        private void Dead()
        {
            m_rb.velocity = Vector2.zero;
            gameObject.layer = LayerMask.NameToLayer(deadLayer.ToString());
        }
    }
}
