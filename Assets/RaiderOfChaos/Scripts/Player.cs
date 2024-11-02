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
            Init();
        }

        private void Update()
        {
            ActionHandle();
        }

        public override void Init()
        {
            LoandStat();

            m_fsm.ChangeState(PlayerState.Idle);
            m_preState = PlayerState.Idle;
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
            if (IsAttacking || IsDashing || m_isKnockBack || IsDead) return;

            if (GamepadManager.Ins.IsStatic)
            {
                m_curSpeed = 0f;
                m_rb.velocity = new Vector2(m_curSpeed,m_rb.velocity.y);
                if (!m_isInvincible)
                {
                    ChangeState(PlayerState.Idle);
                }
            }
            else
            {
                if (GamepadManager.Ins.CanAttack)
                {
                    if (!m_isAttacked)
                    {
                        m_isAttacked = true;
                        ChangeState(PlayerState.Attack);
                    }
                }else if (GamepadManager.Ins.CanUlti && m_curEnergy >= m_curStat.ultiEnergy)
                {

                }
            }

            ReduceActionRate(ref m_isDashed,ref m_curDashRate,ref m_curStat.dashRate);
            ReduceActionRate(ref m_isAttacked,ref m_curAttackRate,ref m_curStat.atkRate);
        }

        private void Move(Direction dir)
        {
            if (m_isKnockBack) return;
            if (dir == Direction.Left || dir == Direction.Right)
            {
                Flip(dir);

                m_hozDir = dir == Direction.Left ? -1 : 1;

                if (GameManager.Ins.setting.isOnMobile)
                {
                    m_rb.velocity = new Vector2(GamepadManager.Ins.joystick.xValue * m_curSpeed,m_rb.velocity.y);
                }
                else
                {
                    m_rb.velocity = new Vector2(m_hozDir * m_curSpeed, m_rb.velocity.y);
                }
            }
        }

        public override void Dash()
        {
            if (IsFacingLeft)
            {
                transform.position = new Vector3(transform.position.x - m_curStat.dashDist,
                    transform.position.y,transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + m_curStat.dashDist,
                    transform.position.y, transform.position.z);
            }
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
        private void Idle_Enter()
        {
            ActiveCol(PlayerCollider.Normal);
        }
        private void Idle_Update()
        {
            if(GamepadManager.Ins.CanMoveLeft || GamepadManager.Ins.CanMoveRight)
            {
                ChangeState(PlayerState.Walk);
            }

            if (IsDead)
            {
                ChangeState(PlayerState.Dead);
            }

            Helper.PlayAnim(m_amin, PlayerState.Idle.ToString());
        }
        private void Idle_Exit() { }
        private void Walk_Enter()
        {
            m_curSpeed = m_curStat.moveSpeed;
        }
        private void Walk_Update()
        {
            if (GamepadManager.Ins.CanDash)
            {
                if (!m_isDashed)
                {
                    m_isDashed = true;
                    ChangeState(PlayerState.Dash);
                }
            }
            else
            {
                m_curSpeed += Time.deltaTime * 1.5f;
                m_curSpeed = Mathf.Clamp(m_curSpeed, m_curStat.moveSpeed, m_curStat.runSpeed);
                if(m_curSpeed >= m_curStat.runSpeed)
                {
                    Helper.PlayAnim(m_amin, PlayerState.Run.ToString());
                }
                else
                {
                    Helper.PlayAnim(m_amin, PlayerState.Walk.ToString());
                }
            }

            if (GamepadManager.Ins.CanMoveLeft)
            {
                Move(Direction.Left);
            }else if (GamepadManager.Ins.CanMoveRight)
            {
                Move(Direction.Right);
            }

            
        }
        private void Walk_Exit() { }
        private void Run_Enter() { }
        private void Run_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Run.ToString());
        }
        private void Run_Exit() { }
        private void Attack_Enter()
        {
            ChangeStateDelay(PlayerState.Idle);
        }
        private void Attack_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Attack.ToString());
        }
        private void Attack_Exit() { }
        private void Jump_Enter() { }
        private void Jump_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Jump.ToString());
        }
        private void Jump_Exit() { }
        private void DoubleJump_Enter() { }
        private void DoubleJump_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.DoubleJump.ToString());
        }
        private void DoubleJump_Exit() { }
        private void Hit_Enter() { }
        private void Hit_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Hit.ToString());
        }
        private void Hit_Exit() { }
        private void Fall_Enter() { }
        private void Fall_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Fall.ToString());
        }
        private void Fall_Exit() { }
        private void Dead_Enter() { }
        private void Dead_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Dead.ToString());
        }
        private void Dead_Exit() { }
        private void Ultimate_Enter() { }
        private void Ultimate_Update()
        {
            Helper.PlayAnim(m_amin, PlayerState.Ultimate.ToString());
        }
        private void Ultimate_Exit() { }
        private void Dash_Enter()
        {
            gameObject.layer = invincibleLayer;
            ChangeStateDelay(PlayerState.Idle);
        }
        private void Dash_Update() 
        {
            Helper.PlayAnim(m_amin, PlayerState.Dash.ToString());
        }
        private void Dash_Exit() 
        {
            gameObject.layer = normalLayer;
        }

        #endregion
    }
}
