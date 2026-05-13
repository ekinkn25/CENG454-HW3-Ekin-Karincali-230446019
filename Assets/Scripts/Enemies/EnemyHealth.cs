using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Patterns.Observer;
using System.Xml.Serialization;

namespace CoreBreach.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable, IPoolable
    {
        [SerializeField] private float maxHealth=30f;
        [SerializeField] private float currentHealth;
        private bool _isDead = false; // to prevent 2nd die event
        private bool _killedByPlayer  = false;


        
        //IPoolable -it will call when you pull on pool- life and flag become zero
        //wave manager will call this method automatically
        public void OnSpawn()
        {
            _killedByPlayer = false;
            currentHealth = maxHealth;
            _isDead = false;
            gameObject.SetActive(true);
            Debug.Log($"[Enemy Health] did spawn. Life: {currentHealth}");
        }
        
        public void OnDespawn()
        {
            gameObject.SetActive(false);
            Debug.Log($"[EnemyHealth] returned to ppol : {gameObject.name}");
            Destroy(gameObject);
        }

        private void Start()
        {
            // EnemyPool yazılınca bu Start() kaldırılacak
            // Pool, spawn ederken OnSpawn()'ı kendisi çağıracak
            // TODO: EnemyPool eklenince Start()'ı sil
            OnSpawn();
        }


        //IDamagable
        public void TakeDamage(float amount)
        {
            if (_isDead) return;
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);
            Debug.Log($"[EnemyHealth] taken damage: {amount} | Remaining life: {currentHealth} /{maxHealth}");

            if (IsDead())
            {
                _isDead = true;
                HandleDeath();
            }
        }

        public bool IsDead()
        {
            return currentHealth <= 0f;
        }

        private void HandleDeath()
        {
            Debug.Log($"[EnemyHealth] Enemy died: {gameObject.name}");
            //OBSERVER: WaveManager kalan düşman sayısını azaltır Score MAnager puan ekler HUDController Score günceller ama bu sınıf onların hiç birini tanımıypr sadece duyuruyor
            
            GameEvents.OnEnemyDied?.Invoke(transform.position, _killedByPlayer);
            OnSpawn();
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
        public void MarkAsKilledByPlayer()
        {
            _killedByPlayer = true;
        }

        [ContextMenu("Test: 10 hasar ver")]
        private void TestTakeDamage()
        {
            TakeDamage(10f);
        }
    }
}
