using UnityEngine;
using CoreBreach.Player;
using CoreBreach.Patterns.Decorator;
using CoreBreach.Interfaces;

namespace CoreBreach.Player
{
    public class WeaponPickup : MonoBehaviour
    {
        // Choose which power-up on inspector
        public enum PickupType { DoubleDamage, RapidFire }

        [SerializeField] private PickupType pickupType = PickupType.DoubleDamage;

        // TODO: Timer sistemi eklenince decorator otomatik kalkacak
        [SerializeField] private float duration = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            PlayerShooter shooter = other.GetComponent<PlayerShooter>();
            if (shooter == null) return;

            IWeaponBehavior current = shooter.GetCurrentWeapon();

            switch (pickupType)
            {
                case PickupType.DoubleDamage:
                    shooter.SetWeapon(new DoubleDamageDecorator(current));
                    Debug.Log("[WeaponPickup] DoubleDamage aktif!");
                    break;

                case PickupType.RapidFire:
                    shooter.SetWeapon(new RapidFireDecorator(current));
                    Debug.Log("[WeaponPickup] RapidFire aktif!");
                    break;
            }

            // disable pickup : it can only used once
            gameObject.SetActive(false);
        }
    }
}
