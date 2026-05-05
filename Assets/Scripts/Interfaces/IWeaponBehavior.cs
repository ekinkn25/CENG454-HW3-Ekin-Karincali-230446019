using UnityEngine;

namespace CoreBreach.Interfaces
{
    //what problem is it solving:
    //A base class for Decorators. Weapon behavior should start with “default fire” and be modifiable.

    //where are we going to use:
    //BasicWeapon, DoubleDamageDecorator, FireRateDecorator
    public interface IWeaponBehavior
    {
        void Fire(Vector3 origin, Vector3 direction);

        float GetDamage(); 
        //return the current damage value
    }
}