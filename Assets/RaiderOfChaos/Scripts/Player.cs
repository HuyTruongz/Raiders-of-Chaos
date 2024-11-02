using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System;

namespace hyhy.RaidersOfChaos
{
    public class Player : Actor
    {
        [Header("Collider:")]
        public Collider2D headCol;
        public Collider2D bodyCol;
        public Collider2D deadCol;

        private PlayerStat m_curStat;
        private StateMachine<PlayerState> m_fsm;
        private PlayerState m_preState;
        private int m_hozDir, m_verDir;
        private bool m_isDashed;
        private bool m_isAttacked;
        private float m_curDashRate;
        private float m_curAttackRate;
        private float m_curEnergy;

        public PlayerStat CurStat { get => m_curStat; set => m_curStat = value; }
        public StateMachine<PlayerState> Fsm { get => m_fsm; }
        public float CurEnergy { get => m_curEnergy; set => m_curEnergy = value; }

        public bool IsDead
        {
            get => m_fsm.State == PlayerState.Dead || m_preState == PlayerState.Dead;
        }

        public bool IsAttacking
        {
            get => m_fsm.State == PlayerState.Attack || m_fsm.State == PlayerState.Ultimate;
        }
        public bool IsUlti
        {
            get => m_fsm.State == PlayerState.Ultimate;
        }

        public bool IsDashing
        {
            get => m_fsm.State == PlayerState.Dash;
        }

        protected override void Awake()
        {
            base.Awake();
            m_fsm = StateMachine<PlayerState>.Initialize(this);
            if (stat)
            {
                m_curStat = (PlayerStat)stat;
            }
        }

        private void Start()
        {
            FSM_MethodGen.Gen<PlayerState>();
        }

        public override void Init()
        {
            LoandStat();

            m_fsm.ChangeState(PlayerState.Idle);
            ChangeState(PlayerState.Idle);
        }

        private void LoandStat()
        {
            if (m_curStat)
            {
                m_curStat.Load(GameData.Ins.curPlayerId);
            }

            m_curSpeed = m_curStat.moveSpeed;
            m_curHp = m_curStat.hp;
            m_curDmg = m_curStat.damage;
            m_curDashRate = m_curStat.dashRate;
            m_curAttackRate = m_curStat.atkRate;
            m_curEnergy = m_curStat.ultiEnergy;
        }

        private void ActionHandle()
        {

        }

        public void ChangeState(PlayerState state)
        {
            m_preState = m_fsm.State;
            m_fsm.ChangeState(state);
        }

        private IEnumerator ChangeStateDelayCo(PlayerState newState, float timeExtra = 0)
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

        private void ChangeStateDelay(PlayerState newState, float timeExtra = 0)
        {
            StartCoroutine(ChangeStateDelayCo(newState, timeExtra));
        }

        private void ActiveCol(PlayerCollider collider)
        {
            if (headCol)
            {
                headCol.enabled = collider == PlayerCollider.Normal;
            }

            if (bodyCol)
            {
                bodyCol.enabled = collider == PlayerCollider.Normal;
            }

            if (deadCol)
            {
                deadCol.enabled = collider == PlayerCollider.Dead;
            }
        }

        protected override void Dead()
        {
            base.Dead();
            ChangeState(PlayerState.Dead);
        }

        public override void TakeDamaged(float dmg, Actor whoHit)
        {
            base.TakeDamaged(dmg - m_curStat.defense, whoHit);
            if (IsDead || IsUlti) return;
            if(m_curHp > 0 && !m_isInvincible)
            {
                ChangeState(PlayerState.Hit); 
            }
        }

        public void AddXp(float xp)
        {
            m_curStat.xp += xp;

            StartCoroutine(m_curStat.LevelUpCo(
                () =>
                {
                    m_curHp = m_curStat.hp;
                }));
        }

        public void AddEnergy(float energyBouns)
        {
            m_curEnergy += energyBouns;
        }

        #region
        private void Idle_Enter() { }
        private void Idle_Update() { }
        private void Idle_Exit() { }
        private void Walk_Enter() { }
        private void Walk_Update() { }
        private void Walk_Exit() { }
        private void Run_Enter() { }
        private void Run_Update() { }
        private void Run_Exit() { }
        private void Attack_Enter() { }
        private void Attack_Update() { }
        private void Attack_Exit() { }
        private void Jump_Enter() { }
        private void Jump_Update() { }
        private void Jump_Exit() { }
        private void DoubleJump_Enter() { }
        private void DoubleJump_Update() { }
        private void DoubleJump_Exit() { }
        private void Hit_Enter() { }
        private void Hit_Update() { }
        private void Hit_Exit() { }
        private void Fall_Enter() { }
        private void Fall_Update() { }
        private void Fall_Exit() { }
        private void Dead_Enter() { }
        private void Dead_Update() { }
        private void Dead_Exit() { }
        private void Ultimate_Enter() { }
        private void Ultimate_Update() { }
        private void Ultimate_Exit() { }
        private void Dash_Enter() { }
        private void Dash_Update() { }
        private void Dash_Exit() { }

        #endregion
    }
}
