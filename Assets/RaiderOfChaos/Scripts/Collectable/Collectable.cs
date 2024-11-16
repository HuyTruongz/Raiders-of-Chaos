using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hyhy.SPM;

namespace hyhy.RaidersOfChaos
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Collectable : MonoBehaviour
    {
        public int minBouns;
        public int maxBouns;
        public int lifeTime;
        public float spawnForce;
        public AudioClip hitSound;
        public bool deactiveWhenHitted;

        protected int m_bonus;
        private Player m_player;
        protected bool m_isNotMoving;

        private int m_timeCounting;
        private Rigidbody2D m_rb;
        private FlashVfx m_flashVfx;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody2D>();
            m_flashVfx = GetComponent<FlashVfx>();
        }

        public virtual void Init()
        {
            m_isNotMoving = false;
            m_player = GameManager.Ins.Player;
            m_timeCounting = lifeTime;

            if (!m_player || !m_rb || !m_flashVfx) return;

            m_bonus = Random.Range(minBouns, maxBouns) * (GameData.Ins.curLevelId + 1);

            float randForce = Random.Range(-spawnForce, spawnForce);

            m_rb.velocity = new Vector2(randForce, randForce);

            StartCoroutine(StopMove());

            m_flashVfx.OnCompleted.RemoveAllListeners();
            m_flashVfx.OnCompleted.AddListener(() =>
            {
                gameObject.SetActive(false);
            });

            StartCoroutine(CountingDown());
        }

        public void Trigger()
        {
            TriggerCore();
        }

        protected virtual void TriggerCore()
        {
            //playsound;
            if (deactiveWhenHitted)
            {
                gameObject.SetActive(false);
            }
        }

        private IEnumerator CountingDown()
        {
            while (m_timeCounting > 0)
            {
                yield return new WaitForSeconds(1f);

                m_timeCounting--;

                float timeRate = Mathf.Round((float)m_timeCounting / (float)lifeTime);

                if (timeRate <= 0.3f)
                {
                    m_flashVfx.Flash(m_timeCounting);
                }
            }
        }

        private IEnumerator StopMove()
        {
            yield return new WaitForSeconds(1f);
            m_rb.velocity = Vector2.zero;
            m_isNotMoving = true;
        }
    }
}
