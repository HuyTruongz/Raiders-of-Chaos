using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class PoolingManager : Singleton<PoolingManager>
    {
        public List<Pool> Pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        public override void Awake()
        {
            MakeSingleton(false);
        }

        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in Pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);  // Th�m ??i t??ng v�o h�ng ??i c?a pool
                }

                poolDictionary.Add(pool.tag.ToString(), objectPool);  // Th�m pool v�o dictionary
            }
        }
        public GameObject GetPooledObject(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objToSpawn = poolDictionary[tag].Dequeue();  // L?y ??i t??ng t? h�ng ??i
            objToSpawn.SetActive(true);  // K�ch ho?t ??i t??ng
            poolDictionary[tag].Enqueue(objToSpawn);  // ??a ??i t??ng tr? l?i cu?i h�ng ??i ?? s? d?ng l?i sau n�y

            return objToSpawn;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}
