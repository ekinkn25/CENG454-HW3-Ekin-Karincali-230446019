using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.Core
{
    public class CoreHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;
        private bool _isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;
            _isDead = false;

            //it is for when game started, HUD will show the correct value 
            //sent an event right away
            //TODO: when HUDController coded this line will be active
            GameEvents.OnCoreHealthChanged?.Invoke(currentHealth, maxHealth);
            Debug.Log($"[CoreHealth] Game started. Life: {currentHealth}/{maxHealth}");
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);

            Debug.Log($"[CoreHealth] Damage occured: {amount} | Remaining Life: {currentHealth}");

            //OBSERVER: HUDController and AudioManager listens this evet, this class dont know them just announce to them.
            GameEvents.OnCoreHealthChanged?.Invoke(currentHealth,maxHealth);

            if (IsDead())
            {
                _isDead = true;
                Debug.Log("[CoreHealth] CORE DESTROYED!");
                GameEvents.OnCoreDead?.Invoke();
            }
        }

        public bool IsDead()
        {
            return currentHealth <= 0f;
        }

            //editor healper: you can test it in inspector on "Deal 10 points of damage"
            //TODO: once development is complete you can delete this method
        [ContextMenu("Test: Deal 10 points of damage")]
        private void TestTakeDamage()
        {
                TakeDamage(10f);
        }
    }
}
