using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hyhy.SPM;
using System;

namespace hyhy.RaidersOfChaos
{
    public class Projectile : MonoBehaviour, IDamageCreater
    {
        [Header("Base Setting: ")]
        public GameTag damageTo;
        public float speed;
        public float damage;
        public bool deactiveWhenHitted;

        [PoolerKeys(target = PoolerTarget.NONE)]
        public string bodyHitPool;

        [HideInInspector]
        public Actor owenr;

        private Vector2 m_prevPos;
        private RaycastHit2D[] m_hit;
        private Vector2 m_dir;

        private void OnEnable()
        {
            RefreshLastPos();
        }

        private void Update()
        {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            DealDamage();

            RefreshLastPos();
        }

        public void DealDamage()
        {
            m_dir = (Vector2)transform.position - m_prevPos;
            float distance = m_dir.magnitude;
            m_dir.Normalize();

            m_hit = Physics2D.RaycastAll(m_prevPos, m_dir, distance);

            if(m_hit == null || m_hit.Length <= 0) return;

            for(int i = 0; i < m_hit.Length; i++)
            {
                var hit = m_hit[i];
                if(hit.collider == null) continue;
                if (hit.collider.CompareTag(damageTo.ToString()))
                {
                    Actor actor = hit.collider.GetComponent<Actor>();

                    if (!actor) return;

                    actor.WhoHit = owenr;
                    actor.TakeDamaged(damage, owenr);

                    PoolersManager.Ins.Spawn(PoolerTarget.NONE, bodyHitPool, hit.transform.position, Quaternion.identity);

                    if (deactiveWhenHitted)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }

        }

        private void RefreshLastPos()
        {
            m_prevPos = (Vector2)transform.position;
        }

        private void OnDisable()
        {
            m_hit = new RaycastHit2D[0];
            transform.position = new Vector3(1000f, 1000f, 0);
        }
    }

}