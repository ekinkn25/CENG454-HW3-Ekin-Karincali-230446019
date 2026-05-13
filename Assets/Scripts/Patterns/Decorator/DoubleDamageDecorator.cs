using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Patterns.Decorator
{
    // decorator that doubles the damage 
    //it can be stacked like you can get 2 ow doubleing 
    public class DoubleDamageDecorator : WeaponDecorator
    {
        public DoubleDamageDecorator(IWeaponBehavior wrapped) : base(wrapped)
        {
            Debug.Log($"[DoubleDamageDecorator] Aktif. Hasar: {wrapped.GetDamage()} → {wrapped.GetDamage() * 2}");
        }

        // just this method is overriding. Fire() comes from WeaponDecorator.
        public override float GetDamage()
        {
            return _wrapped.GetDamage() * 2f;
        }
    }
}
