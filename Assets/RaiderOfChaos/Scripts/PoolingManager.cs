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
                    objectPool.Enqueue(obj);  // Thêm ??i t??ng vào hàng ??i c?a pool
                }

                poolDictionary.Add(pool.tag.ToString(), objectPool);  // Thêm pool vào dictionary
            }
        }
        public GameObject GetPooledObject(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objToSpawn = poolDictionary[tag].Dequeue();  // L?y ??i t??ng t? hàng ??i
            objToSpawn.SetActive(true);  // Kích ho?t ??i t??ng
            poolDictionary[tag].Enqueue(objToSpawn);  // ??a ??i t??ng tr? l?i cu?i hàng ??i ?? s? d?ng l?i sau này

            return objToSpawn;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}
