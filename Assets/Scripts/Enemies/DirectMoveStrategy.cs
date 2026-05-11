using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Enemies
{
    //strategy pattern - straight movement
    //EnemyController dont know this class directly just know on the interface of IEnemyMovement 
    //you dont have to change this class to add new strategy
    public class DirectMoveStrategy : MonoBehaviour
    {
        private readonly float _speed;

        public DirectMoveStrategy(float speed)//speed will get from outside(EneemyController) so samestrategy can be used for different speeds
        {
            _speed = speed;
        }

        //it will called by EnemyController every frame
        //enemy move directly to the target
        public void Move(Transform enemyTransform, Transform target)
        {
            if (target == null) return;

            //if core is destroyed dont move
            Vector3 direction = (target.position - enemyTransform.position).normalized;

            direction.y = 0f;

            enemyTransform.position += direction * _speed * Time.deltaTime;

            //turn enemy to the target
            if (direction != Vector3.zero)
            {
                enemyTransform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
