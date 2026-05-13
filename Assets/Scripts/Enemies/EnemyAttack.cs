using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private float attackDamage = 5f;

        // Saniyede kaç kez hasar verir
        // Sürekli temas hasarı — Core'a yapışınca her interval'de vurur
        // [SerializeField] private float attackInterval = 1f;

        // private float _attackTimer = 0f;
        // private IDamageable _currentTarget;
        private EnemyHealth _enemyHealth;
        // ö
        //         private void Update()
        //         {
        //             if (GameEvents.IsGameOver) return;
        //             if (!_isInContact) return;
        //             if (_currentTarget == null) return;
        //             if (_currentTarget.IsDead()) return;

        //             // Her attackInterval saniyede bir hasar ver
        //             _attackTimer -= Time.deltaTime;
        //             if (_attackTimer <= 0f)
        //             {
        //                 _currentTarget.TakeDamage(attackDamage);
        //                 _attackTimer = attackInterval;

        //                 Debug.Log($"[EnemyAttack] Core'a {attackDamage} hasar verildi.");
        //             }
        //         }
        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameEvents.IsGameOver) return;

            // Sadece Core'a çarpınca hasar ver
            // Tag kontrolü — Core'un tag'i "Core" olmalı
            if (!other.CompareTag("Core")) return;

            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable == null) return;
            if (damageable.IsDead()) return;

            damageable.TakeDamage(attackDamage);
            Debug.Log($"[EnemyAttack] Core'a {attackDamage} hasar verdi ve öldü.");
            _enemyHealth.TakeDamage(_enemyHealth.GetCurrentHealth());
            // _currentTarget = damageable;
            // _isInContact   = true;
            // _attackTimer   = 0f; // Hemen ilk hasar
            // Debug.Log("[EnemyAttack] Core'a ulaştı, hasar başlıyor.");
        }

        // private void OnTriggerExit(Collider other)
        // {
        //     if (!other.CompareTag("Core")) return;

        //     _currentTarget = null;
        //     _isInContact   = false;
        //     Debug.Log("[EnemyAttack] Core'dan uzaklaştı.");
        // }
    }
}