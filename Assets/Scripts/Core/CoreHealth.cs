using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Core
{
    public class CoreHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead()) return;

            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);

            Debug.Log($"[CoreHealth] Hasar alındı: {amount} | Kalan can: {currentHealth}");

            if (IsDead())
            {
                Debug.Log("[CoreHealth] CORE DESTROYED! (Observer event buraya gelecek)");
            }
        }

        public bool IsDead()
        {
            return currentHealth <= 0f;
        }
    }
}
