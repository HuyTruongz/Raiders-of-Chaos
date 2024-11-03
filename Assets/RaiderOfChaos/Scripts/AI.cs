using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System;

namespace hyhy.RaidersOfChaos
{
    public class AI : Actor
    {
        [HideInInspector]
        public bool isBoss;
        public AIStat bossStat;
        public float actionDist;
        public float ultiDist;
        public float dashDist;

        protected AIStat m_curStat;
        protected Vector3 m_targetDir;
        protected Player m_player;
        protected AIState m_prevState;
        protected float m_actionRate;
        protected StateMachine<AIState> m_fsm;
        protected float m_curAtkTiem;
        protected float m_curDashTime;
        protected float m_curUltiTime;
        private bool m_isAtked;
        private bool m_isDashed;
        private bool m_isUltied;

        public StateMachine<AIState> Fsm { get => m_fsm; }

        protected bool IsDead
        {
            get => m_fsm.State == AIState.Dead || m_prevState == AIState.Dead;
        }

        protected bool IsAtking
        {
            get => m_fsm.State == AIState.Attack || m_fsm.State == AIState.Ultimate;
        }

        protected bool CanAction
        {
            get => Vector2.Distance(m_player.transform.position, transform.position) <= actionDist;
        }

        protected bool CanUlti
        {
            get => Vector2.Distance(m_player.transform.position, transform.position) <= ultiDist;
        }

        protected bool IsDashing
        {
            get => m_fsm.State == AIState.Dash;
        }

        protected override void Awake()
        {
            base.Awake();
            FSMInit(this);
        }

        private void Start()
        {
            Init();
        }

        protected void FSMInit(MonoBehaviour behaviour)
        {
            m_fsm = StateMachine<AIState>.Initialize(behaviour);
        }

        public override void Init()
        {
            if (!stat) return;

            if (!isBoss)
            {
                m_curStat = (AIStat)stat;
            }
            else if (bossStat)
            {
                m_curStat = bossStat;
            }

            m_player = GameManager.Ins.Player;
            m_curSpeed = m_curStat.moveSpeed;
            m_curAtkTiem = m_curStat.atkTime;
            m_curDashTime = m_curStat.dashTime;
            m_curUltiTime = m_curStat.ultiTime;
            m_curHp = m_curStat.CurHp;
            m_curDmg = m_curStat.CurDmg;
            m_isInvincible = false;
            m_isKnockBack = false;
            GetActionRate();
            m_fsm.ChangeState(AIState.Walk);
            m_prevState = m_fsm.State;

            //CreateHealthBarUI();
        }

        private void GetActionRate()
        {
            m_actionRate = UnityEngine.Random.Range(0f, 1f);
        }

        public void ChangeState(AIState state)
        {
            m_prevState = m_fsm.State;
            m_fsm.ChangeState(state);
        }

        private IEnumerator ChangeStateDelayCo(AIState newState, float timeExtra = 0)
        {
            var animClip = Helper.GetClip(m_amin, m_fsm.State.ToString());
            if (animClip)
            {
                yield return new WaitForSeconds(animClip.length + timeExtra);
                if (!IsDead)
                {
                    ChangeState(newState);
                }
            }

            yield return null;
        }

        private void ChangeStateDelay(AIState newState, float timeExtra = 0)
        {
            StartCoroutine(ChangeStateDelayCo(newState, timeExtra));
        }

        public override void TakeDamaged(float dmg, Actor whoHit)
        {
            if (IsDead) return;
            base.TakeDamaged(dmg, whoHit);
            if(m_curHp > 0 && !m_isInvincible)
            {
                ChangeState(AIState.Hit);
            }
        }

        protected override void Dead()
        {
            base.Dead();
            ChangeState(AIState.Dead);
        }

        protected void GetTargetDir()
        {
            m_targetDir = m_player.transform.position - transform.position;
            m_targetDir.Normalize();
        }

        #region FSM
        private void Walk_Enter() { }
        private void Walk_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Walk.ToString());
        }
        private void Walk_Exit() { }
        private void Dash_Enter() { }
        private void Dash_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Dash.ToString());
        }
        private void Dash_Exit() { }
        private void Ultimate_Enter() { }
        private void Ultimate_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Ultimate.ToString());
        }
        private void Ultimate_Exit() { }
        private void Attack_Enter() { }
        private void Attack_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Attack.ToString());
        }
        private void Attack_Exit() { }
        private void Dead_Enter() { }
        private void Dead_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Dead.ToString());
        }
        private void Dead_Exit() { }
        private void Hit_Enter() { }
        private void Hit_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Hit.ToString());
        }
        private void Hit_Exit() { }
        #endregion
    }
}
