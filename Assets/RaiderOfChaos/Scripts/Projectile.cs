using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hyhy.SPM;
using System;

namespace hyhy.RaidersOfChaos
{
    public class Projectile : MonoBehaviour, IDamageCreater
    {
        [Header("Base Setting:")]
        public GameTag damageTo;
        public float speed;
        public float damage;
        public bool deactiveWhenHitted;

        [PoolerKeys(target = PoolerTarget.NONE)]
        public string bodyHitPool;

        [HideInInspector]
        public Actor owner;

        private Vector2 m_prevPos;
        private RaycastHit2D m_hit;
        private Vector2 m_dir;

        private void Update()
        {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            DealDamage();

            RefereshLastPos();
        }

        private void OnEnable()
        {
            RefereshLastPos();
        }

        public void DealDamage()
        {
            m_dir = (Vector2)transform.position - m_prevPos;
            float distance = m_dir.magnitude;
            m_dir.Normalize();

            m_hit = Physics2D.Raycast(m_prevPos, m_dir, distance);

            if(!m_hit && !m_hit.collider) return;

            var col = m_hit.collider;

            Checking(col);
        }

        private void Checking(Collider2D col)
        {
            if (col.CompareTag(damageTo.ToString()))
            {
                Actor actor = col.GetComponent<Actor>();

                if(!actor) return;

                actor.WhoHit = owner;
                actor.TakeDamaged(damage,owner);

                //var rotate = transform.localScale.x < 0 ?
                //    Quaternion.Inverse(transform.rotation) : 
                //    transform.rotation;

                PoolersManager.Ins.Spawn(PoolerTarget.NONE, bodyHitPool, m_hit.transform.position
                   , Quaternion.identity);

                if (deactiveWhenHitted)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void RefereshLastPos()
        {
            m_prevPos = (Vector2)transform.position;
        }

        private void OnDisable()
        {
            m_hit = new RaycastHit2D();
            transform.position = new Vector3(1000f,1000f,0);
        }
    }

}