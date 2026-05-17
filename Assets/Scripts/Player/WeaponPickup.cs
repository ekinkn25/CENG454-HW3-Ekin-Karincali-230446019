using UnityEngine;
using CoreBreach.Player;
using CoreBreach.Patterns.Decorator;
using CoreBreach.Interfaces;
using System.Collections;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.Player
{
    public class WeaponPickup : MonoBehaviour
    {
        // Choose which power-up on inspector
        public enum PickupType { DoubleDamage, RapidFire }

        [Header("Pickup Ayarları")]
        [SerializeField] private PickupType pickupType = PickupType.DoubleDamage;

        // TODO: Timer sistemi eklenince decorator otomatik kalkacak
        [SerializeField] private float duration = 10f;
        [SerializeField] private float      rotationSpeed = 90f;
        private bool _isPickedUp = false;

        private void Update()
        {
             transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (GameEvents.IsGameOver) return;
            if(_isPickedUp) return;
            if (!other.CompareTag("Player")) return;

            PlayerShooter shooter = other.GetComponent<PlayerShooter>();
            if (shooter == null) return;

            _isPickedUp = true;

            // save current weapon - it will load when when duration ends
            IWeaponBehavior originalWeapon = shooter.GetCurrentWeapon();

            // make choosen decorator and apply it
            IWeaponBehavior newWeapon = CreateDecorator(originalWeapon);
            shooter.SetWeapon(newWeapon);

            Debug.Log($"[WeaponPickup] {pickupType} aktif! Süre: {duration}s");

            // Görsel olarak gizle ama objeyi aktif tut — Coroutine çalışsın
            GetComponent<Renderer>().enabled  = false;
            GetComponent<Collider>().enabled  = false;
            
            // if there is duration limit undo it
            if (duration > 0f)
            {
                StartCoroutine(RevertAfterDuration(shooter, originalWeapon, newWeapon));
            }

            // hide the pick image
            // gameObject.SetActive(false);
        }

        private IWeaponBehavior CreateDecorator(IWeaponBehavior current)
        {
            switch (pickupType)
            {
                case PickupType.DoubleDamage:
                    return new DoubleDamageDecorator(current);

                case PickupType.RapidFire:
                    return new RapidFireDecorator(current);

                default:
                    return current;
            }
        }

        // when timer runs out switch back to the original weapon
        private IEnumerator RevertAfterDuration(
            PlayerShooter shooter,
            IWeaponBehavior originalWeapon,
            IWeaponBehavior decoratedWeapon)
        {
            yield return new WaitForSeconds(duration);

            // RapidFire restores the rapidFire
            if (decoratedWeapon is RapidFireDecorator rapidFire)
            {
                rapidFire.Revert();
            }

            // return back to original weapon
            shooter.SetWeapon(originalWeapon);

            Debug.Log($"[WeaponPickup] {pickupType} süresi doldu, orijinal silaha dönüldü.");
            // Coroutine bitti, şimdi tamamen kapat
            gameObject.SetActive(false);
        }
    }
}