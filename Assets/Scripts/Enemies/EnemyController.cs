using UnityEngine;
using CoreBreach.Interfaces;
using CoreBreach.Enemies;
namespace CoreBreach.Interfaces{

    //strategy pattern : this class dont directly know DirectMoveStrategy or ZigzagMoveStrategy. Just knows IEnemyMovementStrategy intergace
    //so to add new strategy we dont have to change this class
    public class EnemyController : MonoBehaviour
    {
        [Header("Hareket Ayarları")]
        [SerializeField] private float moveSpeed = 3f;

        //The target of the enemy (arrenges boy inspector or WaveManager)
        //TODO: when WaveManager spwaned this referance will be assigned automatically
        [Header("Hedef")]
        [SerializeField] private Transform coreTransform;

        //strategy referance: not concrete type it holds interface type
        // //so this way using SetStrategy() we can change it in runtime
        private IEnemyMovementStrategy _movementStrategy;
        private EnemyHealth _enemyHealth;
        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            //default strategy: direct move
            //TODO: when WaveMAnager spawned SetStrategt() will be called untill then default strategy will be active.
            SetStrategy(new DirectMoveStrategy(moveSpeed));
        }

        private void Update()
        {
            //if enemy died dont move
            if(_enemyHealth != null && _enemyHealth.IsDead()) return;

            //if strategy is not assigned dont move
            if (_movementStrategy == null) return;

            //if there is no target then dont move
            if (coreTransform == null)
            {
                Debug.LogWarning("[EnemyController] You didnt assign Core a target");
                return;
            }
            //dont matter strategy this will be runned this line dont have to knoe if it is direct or zigzag
            _movementStrategy.Move(transform, coreTransform);
        }

        //for changin strategy from outside: WaveManager will be used
        public void SetStrategy(IEnemyMovementStrategy strategy)
        {
            _movementStrategy = strategy;
            Debug.Log($"[EnemyController] Strategy assigned: {strategy.GetType().Name}");
        }

        //it will tell the target when WaveManager spawning
        //TODO: WaveMAnager will call this method
        public void SetTarget(Transform target)
        {
            coreTransform = target;
        }
    }
}
