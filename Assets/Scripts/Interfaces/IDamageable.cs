namespace CoreBreach.Interfaces
{
    //what problem is it solving:
    //A bullet can hit both an enemy and a core. A bullet shouldn't ask, “Is this an enemy or a core?” It should only ask, “Can it take damage?”

    //where are we going to use:
    //EnemyHealth and CoreHealth Class

    public interface IDamageable
    {
        void TakeDamage(float amount);
        bool IsDead();
    }
}