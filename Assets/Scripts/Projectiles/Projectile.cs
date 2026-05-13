using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifetime = 3f;

        private float _damage;
        // private Vector3 _direction;
        private float _lifetimeTimer;
        private Rigidbody _rb;
        private Collider  _collider;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        //IPoolable : it calls when it pulls from pool, all states will be zero 

        public void OnSpawn()
        {
            _lifetimeTimer=lifetime;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _collider.enabled = true;
            gameObject.SetActive(true);

            Debug.Log("[Projectile] goes from pool and spawned");
        }

        public void OnDespawn()
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;  
            _collider.enabled   = false;
            gameObject.SetActive(false);

            Debug.Log("[Projectile] Havuza döndü."); 
        }

        public void Initialize(Vector3 direction, float damage) //does an dependency injection
        {
            // _direction = direction.normalized;
            _damage = damage;
            _rb.linearVelocity = direction.normalized * speed;
        }

        private void Update()
        {
            // transform.position += _direction * speed * Time.deltaTime;
            if (GameEvents.IsGameOver)
            {
                ReturnToPool();
                return;
            }

            _lifetimeTimer -= Time.deltaTime;
            if (_lifetimeTimer <= 0f)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameEvents.IsGameOver)
            {
                ReturnToPool();
                return;
            }

            if (other.CompareTag("Player")) return;

            Debug.Log($"[Projectile] Hitted: {other.name} | Tag: {other.tag}");

            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                if (damageable.IsDead())
                {
                    ReturnToPool();
                    return;
                }
                damageable.TakeDamage(_damage);
                Debug.Log($"[Projectile] gave {other.name} to {_damage} point damages.");
            }

            ReturnToPool();
        }

        // TODO: Phase 6.2 — ProjectilePool yazılınca
        //       Destroy satırını şununla değiştir:
        //       ProjectilePool.Instance.ReturnToPool(this);
        private void ReturnToPool()
        {
            // TODO: Phase 6.2 sonrası bu satır kaldırılacak
            Destroy(gameObject);
        }
    }
}
