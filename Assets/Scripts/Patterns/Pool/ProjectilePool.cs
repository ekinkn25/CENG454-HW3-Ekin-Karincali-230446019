using UnityEngine;
using System.Collections.Generic;
using CoreBreach.Projectiles;

namespace CoreBreach.Patterns.Pool
{
    public class ProjectilePool : MonoBehaviour
    {
        //SINGLETON: all Projectiles using the same pool, it dont make sense that more than one ProjectilePool so Singleton justifed.
        public static ProjectilePool Instance {get; private set; }
        [Header("Pool Ayarları")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int poolSize = 20;
        private Queue<Projectile> _pool;

        private void Awake()
        {
            //checking if it hasv two destroy the second
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            InitializePool();
        }

        //start the pool : begining of the game it make poolSize much projectile, first disable them and add them to queue so in first fire we dont make new object
        private void InitializePool()
        {
            _pool = new Queue<Projectile>();
            for (int i = 0; i< poolSize; i++)
            {
                Projectile p = CreateNewProjectile();
                p.OnDespawn();
                _pool.Enqueue(p);
            }
            Debug.Log($"[ProjectilePool] you started with {poolSize} projectiles and they are on wait-list.");

        }

        //get projectile from pool, use if there is a projectile in queue and if dont have projectile in queue make a new one
        public Projectile GetFromPool()
        {
            Projectile p;
            if(_pool.Count > 0)
            {
                p = _pool.Dequeue();
                Debug.Log($"[ProjectilePool] got from pool. Remainin proectiles: {_pool.Count}");
            }
            else
            {
                // TODO: poolSize'ı artırmayı düşün, bu durum sık olmamalı
                p = CreateNewProjectile();
                Debug.LogWarning("[ProjectilePool] Empty pool, created a new projectile!");
            }
            p.OnSpawn();
            return p;
        }
        
        //return projectile to the pool: projectile is not destroyed, its disabled and returned back to the queue
        public void ReturnToPool(Projectile p)
        {
            p.OnDespawn();
            _pool.Enqueue(p);
            Debug.Log($"[ProejctilePool] Returned to the pool. Total: {_pool.Count}");
        }

        private Projectile CreateNewProjectile()
        {
            GameObject obj = Instantiate(projectilePrefab, transform);
            Projectile p = obj.GetComponent<Projectile>();
            if(p == null)
            {
                Debug.LogError("[ProjectilePool] Prefab dont have Projectile script!");
            }
            return p;
        }
    }
}
