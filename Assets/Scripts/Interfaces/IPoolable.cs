namespace CoreBreach.Interfaces
{
    //what problem is it solving:
    //Object Pool needs to know how to reset an object that is returned to the pool. However, regardless of the object’s type (bullet, enemy), it should be able to be reset in the same way.

    //where are we going to use:
    //Projectile, Enemy
    public interface IPoolable
    {
        void OnSpawn();
        //it is called when removed from the pool (enable)

        void OnDespawn();
        //It is called when returning to the pool (disable)
    }
}