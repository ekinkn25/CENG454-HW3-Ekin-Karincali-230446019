using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifetime = 3f;

        private float _damage;
        // private Vector3 _direction;
        private float _lifetimeTimer;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Initialize(Vector3 direction, float damage) //does an dependency injection
        {
            // _direction = direction.normalized;
            _damage = damage;
            _lifetimeTimer = lifetime;
            _rb.linearVelocity = direction.normalized * speed;
        }

        private void Update()
        {
            // transform.position += _direction * speed * Time.deltaTime;

            _lifetimeTimer -= Time.deltaTime;
            if (_lifetimeTimer <= 0f)
            {
                Destroy(gameObject); // TODO: Replace with pool return
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;

            Debug.Log($"[Projectile] Hitted: {other.name} | Tag: {other.tag}");

            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                if (damageable.IsDead())
                {
                    Destroy(gameObject); //TODO: replace with pool return
                    return;
                }
                damageable.TakeDamage(_damage);
                Debug.Log($"[Projectile] gave {other.name} to {_damage} point damages.");
            }

            Destroy(gameObject); // TODO: Replace with pool return
        }
    }
}
