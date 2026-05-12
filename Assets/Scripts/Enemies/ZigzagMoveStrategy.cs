using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Enemies
{
    //strategy Pattern - Zigzag movement
    //hedefe doğru giderken sinüs eğrisitle yana sallanır
    public class ZigzagMoveStrategy : IEnemyMovementStrategy
    {
        private readonly float _speed;
        private readonly float _frequency; //how fast the sine wave oscillates
        private readonly float _amplitude; //how far it deviates from the center (in meters)

        public ZigzagMoveStrategy(float speed, float frequency = 2f, float amplitude = 1.5f)
        {
            _speed = speed;
            _frequency = frequency;
            _amplitude = amplitude;
        }

        public void Move(Transform enemyTransform, Transform target)
        {
            if (target == null) return;

            Vector3 directionToTarget = (target.position- enemyTransform.position).normalized;
            directionToTarget.y=0f;

            //right vector: perpendicular to the main way
            //Vector3.Cross(A, B )-> A ve B'ye dik olan vektörü verir
            Vector3 rightVector = Vector3.Cross(directionToTarget, Vector3.up);

            //sine wave : oscillates between -1 and +1
            //frequency + , oscilating rapidly 
            //amplitude + , oscilating widely
            float sideOffset = Mathf.Sin(Time.time * _frequency) * _amplitude;

            //final movement
            Vector3 moveDirection = directionToTarget + rightVector * sideOffset;
            moveDirection.y = 0f;
            moveDirection.Normalize();

            enemyTransform.position += moveDirection * _speed * Time.deltaTime;
            
            //return enemy to the movement direction
            if (moveDirection != Vector3.zero)
            {
                enemyTransform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }

}
