using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Player;

namespace CoreBreach.Patterns.Decorator
{
    // increasing fire rate
    public class RapidFireDecorator : WeaponDecorator
    {
        private readonly float _originalFireRate;
        private readonly BasicWeapon _basicWeapon;

        public RapidFireDecorator(IWeaponBehavior wrapped, float multiplier = 2f) : base(wrapped)
        {
            // check if wrapped weapon is BasicWeapon
            // Pattern: "is" ile tip kontrolü — concrete tipe bağımlılık minimum
            _basicWeapon = wrapped as BasicWeapon;

            if (_basicWeapon != null)
            {
                _originalFireRate      = _basicWeapon.FireRate;
                _basicWeapon.FireRate /= multiplier;   // 0.3 → 0.15 (2x hız)

                Debug.Log($"[RapidFireDecorator] Aktif. FireRate: {_originalFireRate} → {_basicWeapon.FireRate}");
            }
            else
            {
                Debug.LogWarning("[RapidFireDecorator] Sarılan silah BasicWeapon değil, hız değiştirilemedi.");
            }
        }

        // Restore the original speed when the decorator's duration expires        
        // TODO: WeaponPickup timer eklenince bu çağrılacak
        public void Revert()
        {
            if (_basicWeapon != null)
            {
                _basicWeapon.FireRate = _originalFireRate;
                Debug.Log("[RapidFireDecorator] Hız eski değerine döndü.");
            }
        }
    }
}