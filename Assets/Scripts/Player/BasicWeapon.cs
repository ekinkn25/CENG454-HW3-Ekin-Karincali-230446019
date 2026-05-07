using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Player
{
    public class BasicWeapon : MonoBehaviour, IWeaponBehavior
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float fireRate = 0.3f;
        [SerializeField] private const float BaseDamage = 10f;
        private float _fireCooldown = 0f;
        
        private void Update()
        {
            if (_fireCooldown > 0f)
            {
                _fireCooldown -= Time.deltaTime;
            }
        }

        public void Fire(Vector3 origin, Vector3 direction)
        {
            if (_fireCooldown > 0f) return;
            if (projectilePrefab == null)
            {
                Debug.LogWarning("[BasicWeapon] Projectile Prefab atanmamış!");
                return;
            }

            GameObject projectileObj = Instantiate(projectilePrefab, origin, Quaternion.identity);
            //Instantiate komutu prefabrik projectile'ı alır ve sahneye gerçek bir obje üretilmesini sağlar, üretilen objenin içindeki projectile scriptini bulur ve Initialize metodu çağrılarak mermi hareketine başlatılır
            //bu işlem CPU için maliyetli ileride bu kısmı Object Pool kullanarak revize edilebilir
            // TODO: Replace with ObjectPool

            Projectiles.Projectile projectile = projectileObj.GetComponent<Projectiles.Projectile>();

            if (projectile != null)
            {
                projectile.Initialize(direction, GetDamage());
            }

            _fireCooldown = fireRate;
            Debug.Log($"[BasicWeapon] Ateş edildi. Hasar: {GetDamage()} | Yön: {direction}");
        }

        public float GetDamage()
        {
            return BaseDamage;
        }
    }
}