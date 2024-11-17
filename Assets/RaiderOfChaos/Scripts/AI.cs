using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System;
using static UnityEditor.PlayerSettings;

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

        private void Update()
        {
            LimitHozMoving();

            if (!m_player)
            {
               m_player = GameManager.Ins.Player;
                return;
            }

            ActionHandle();

            if (m_healthBar)
            {
                m_healthBar.UpdateValue(m_curHp,m_curStat.CurHp);
            }
        }

        private void ActionHandle()
        {
            ReduceActionRate(ref m_isAtked, ref m_curAtkTiem, m_curStat.atkTime);
            ReduceActionRate(ref m_isDashed, ref m_curDashTime, m_curStat.dashTime);
            ReduceActionRate(ref m_isUltied, ref m_curUltiTime, m_curStat.ultiTime);
            if (IsAtking || IsDashing || m_isKnockBack || IsDead || m_player.IsDead) return;

            GetTargetDir();

            if(CanUlti && m_actionRate <= m_curStat.CurUltiRate && !m_isUltied)
            {
                m_isUltied = true;
                ChangeState(AIState.Ultimate);
            }

            if (CanAction)
            {
                ActionSwitch();
            }

            if (m_targetDir.x > 0)
            {
                Flip(Direction.Right);
            }
            else
            {
                Flip(Direction.Left);
            }

            if (isBoss)
            {
                GUIManager.Ins.bossHpBar.UpdateValue(m_curHp,m_curStat.CurHp);
            }
          
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
            gameObject.layer = normalLayer;
            CreateHealthBarUI();

            if (isBoss)
            {
                GUIManager.Ins.bossHpBar.Show(true);
                GUIManager.Ins.bossHpBar.UpdateValue(m_curHp, m_curStat.CurHp);
            }
        }

        private void GetActionRate()
        {
            m_actionRate = UnityEngine.Random.Range(0f, 1f);
        }

        protected void ActionSwitch()
        {
            if(m_actionRate <= m_curStat.dashRate && m_actionRate > m_curStat.CurUltiRate)
            {
                if (m_isDashed) return;

                ChangeState(AIState.Dash);
                m_isDashed = true;
            }
            else if(m_actionRate <= m_curStat.atkRate)
            {
                if(m_isAtked) return;
                m_isAtked = true;
                ChangeState(AIState.Attack);
            }
        }

        public override void Dash()
        {
            if (IsFacingLeft)
            {
                transform.position = new Vector3(transform.position.x - dashDist,
                    transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + dashDist,
                    transform.position.y, transform.position.z);
            }
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
        private void Walk_Enter()
        {
            if (isEnemyFlight)
            {
                m_rb.isKinematic = true;
            }
        }
        private void Walk_Update()
        {
            if (m_isAtked)
            {
                m_rb.velocity = Vector2.zero;
            }
            else
            {
                m_rb.velocity = new Vector2(m_targetDir.x * m_curSpeed,m_rb.velocity.y);
            }
            Helper.PlayAnim(m_amin, AIState.Walk.ToString());
        }
        private void Walk_Exit() { }
        private void Dash_Enter()
        {
            gameObject.layer = invincibleLayer;
            ChangeStateDelay(AIState.Walk);
        }
        private void Dash_Update()
        {
            Helper.PlayAnim(m_amin, AIState.Dash.ToString());
        }
        private void Dash_Exit()
        {
            gameObject.layer = normalLayer;
            GetActionRate();
        }
        private void Ultimate_Enter()
        {
            m_curDmg = m_curStat.CurDmg + m_curStat.CurDmg * 0.3f;
            ChangeStateDelay(AIState.Walk);
        }
        private void Ultimate_Update()
        {
            m_rb.velocity = Vector3.zero;
            Helper.PlayAnim(m_amin, AIState.Ultimate.ToString());
        }
        private void Ultimate_Exit()
        {
            GetActionRate();
            m_curDmg = m_curStat.CurDmg;
        }
        private void Attack_Enter()
        {
            ChangeStateDelay(AIState.Walk);
        }
        private void Attack_Update()
        {
            m_rb.velocity = Vector3.zero;
            Helper.PlayAnim(m_amin, AIState.Attack.ToString());
        }
        private void Attack_Exit() 
        {
            GetActionRate();
        }
        private void Dead_Enter()
        {
            m_rb.isKinematic = false;
            m_player.AddEnergy(m_curStat.EnergyBonus);
            m_player.AddXp(m_curStat.XpBonus);

            WavePlayer waveCtrl = GameManager.Ins.WaveCtr;

            if (waveCtrl)
            {
                waveCtrl.AddEnemyKilled(1);
                GUIManager.Ins.waveBar.UpdateValue(waveCtrl.CurrentWave.enemyKilled,waveCtrl.CurrentWave.totalEnemy);
            }

            float luckChecking = UnityEngine.Random.Range(0f, 1f);
            if (luckChecking <= m_player.CurStat.luck)
            {
                CollectableManager.Ins.Spawn(transform.position);
            }
        }
        private void Dead_Update()
        {
            gameObject.layer = deadLayer;
            if (m_healthBar)
            {
                m_healthBar.Show(false);
            }
            Helper.PlayAnim(m_amin, AIState.Dead.ToString());
        }
        private void Dead_Exit() { }
        private void Hit_Enter()
        {
            GUIManager.Ins.dmgTxtMng.Add($"- {m_dmgTaked.ToString("f2")}",transform,"ai_damage");
        }
        private void Hit_Update()
        {
            m_rb.velocity = Vector3.zero;
            KnockBackMove(0.15f);
            if (!m_isKnockBack)
            {
                ChangeState(AIState.Walk);
            }
            Helper.PlayAnim(m_amin, AIState.Hit.ToString());
        }
        private void Hit_Exit() { }
        #endregion

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + ultiDist,
                transform.position.y,
                transform.position.z));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + dashDist,
                transform.position.y,
                transform.position.z));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + actionDist,
                transform.position.y,
                transform.position.z));
        }
    }
}
