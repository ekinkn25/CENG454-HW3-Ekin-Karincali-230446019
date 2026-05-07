using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifetime = 3f;

        private float _damage;
        private Vector3 _direction;
        private float _lifetimeTimer;

        public void Initialize(Vector3 direction, float damage) //does an dependency injection
        {
            _direction = direction.normalized;
            _damage = damage;
            _lifetimeTimer = lifetime;
        }

        private void Update()
        {
            transform.position += _direction * speed * Time.deltaTime;

            _lifetimeTimer -= Time.deltaTime;
            if (_lifetimeTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
                Debug.Log($"[Projectile] gave {other.name} to {_damage} point damages.");
            }

            Destroy(gameObject); //maybe improve this
        }
    }
}
