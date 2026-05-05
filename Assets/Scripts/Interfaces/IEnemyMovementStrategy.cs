using UnityEngine;

namespace CoreBreach.Interfaces
{
    //what problem is it solving:
    //Different enemy movement types. Without knowing which movement the enemy class will perform, it asks the strategy class.

    //where are we going to use:
    //DirectMoveStrategy and ZigzagMoveStrategy Class

    public interface IEnemyMovementStrategy
    {
        void Move(Transform enemyTransform, Transform target); 
        //we will be calling this every frame
    }
}