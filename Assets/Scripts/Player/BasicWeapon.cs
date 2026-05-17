using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Patterns.Pool;
using CoreBreach.Projectiles;

namespace CoreBreach.Player
{
    public class BasicWeapon : MonoBehaviour, IWeaponBehavior
    {
        [SerializeField] private float fireRate = 0.3f;
        private const float BaseDamage = 10f;
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
            Projectile projectile = ProjectilePool.Instance.GetFromPool();
            //Instantiate komutu prefabrik projectile'ı alır ve sahneye gerçek bir obje üretilmesini sağlar, üretilen objenin içindeki projectile scriptini bulur ve Initialize metodu çağrılarak mermi hareketine başlatılır
            //bu işlem CPU için maliyetli ileride bu kısmı Object Pool kullanarak revize edilebilir

            if (projectile == null)
            {
                Debug.LogError("[BasicWeapon] Pool'dan mermi alınamadı!");
                return;
            }

            Vector3 spawnPosition = origin + Vector3.up * 0.5f;
            projectile.transform.position = spawnPosition;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.Initialize(direction, GetDamage());

            _fireCooldown = fireRate;
            Debug.Log($"[BasicWeapon] Ateş edildi. Hasar: {GetDamage()}");
        }

        public float GetDamage()
        {
            return BaseDamage;
        }
        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }
    }
}