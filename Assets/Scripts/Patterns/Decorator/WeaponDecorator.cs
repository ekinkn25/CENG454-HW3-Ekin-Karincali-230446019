using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Patterns.Decorator
{
    // Base class of all decorators
    // IWeaponBehavior implements this and PlayerShooter knows this as an interface

    // _wrapped: wrapped weapon reference — it can be BasicWeapon or another decorators
    // default behavior: delegate everything to the wrapped weapon
    // sub classes overrides the method that they want to change
    public abstract class WeaponDecorator : IWeaponBehavior
    {
        // Protected: accessible for sub classes
        internal readonly IWeaponBehavior _wrapped;

        protected WeaponDecorator(IWeaponBehavior wrapped)
        {
            _wrapped = wrapped;
        }

        // default: delegate to wrapped weapon
        // if you dont override this will work
        public virtual void Fire(Vector3 origin, Vector3 direction)
        {
            _wrapped.Fire(origin, direction);
        }

        // default: return the value of wrapped weapon's damage
        // DoubleDamageDecorator will override this
        public virtual float GetDamage()
        {
            return _wrapped.GetDamage();
        }
    }
}
