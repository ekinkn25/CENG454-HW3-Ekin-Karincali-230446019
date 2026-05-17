using System.Collections;
using UnityEngine;
using CoreBreach.Enemies;
using CoreBreach.Interfaces;
using CoreBreach.Patterns.Observer;
using CoreBreach.UI;

namespace CoreBreach.Spawning
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        [SerializeField] private WaveData[] waves;

        [Header("Spawn Settings")]
        [SerializeField] private GameObject   enemyPrefab;
        [SerializeField] private Transform[]  spawnPoints;
        [SerializeField] private Transform    coreTransform;
        [SerializeField] private float       enemySpeed = 5f;

        [Header("Power-Up Management")]
        [SerializeField] private GameObject  doubleDamagePrefab;
        [SerializeField] private GameObject  rapidFirePrefab;
        [SerializeField] private Transform[] powerUpSpawnPoints;

        [Header("HUD")]
        [SerializeField] private HUDController hudController;


        //Current wave status
        private int  _currentWaveIndex   = 0;
        private int  _remainingEnemies   = 0;
        private bool _isSpawning         = false;
        private bool _allWavesDone       = false;
        private GameObject _activePowerUpA;
        private GameObject _activePowerUpB;

        // ---------------------------------------------
        // UNITY LIFECYCLE
        // ---------------------------------------------

        private void OnEnable()
        {
            // Observer: get info when enemy dies
            GameEvents.OnEnemyDied += HandleEnemyDied;
        }

        private void OnDisable()
        {
            // Ghost subscriber prevention
            GameEvents.OnEnemyDied -= HandleEnemyDied;
        }

        private void Start()
        {
            if (waves == null || waves.Length == 0)
            {
                Debug.LogError("[WaveManager] There is no wave data!");
                return;
            }

            StartCoroutine(StartWave(_currentWaveIndex));
        }

        // ---------------------------------------------
        // OBSERVER HANDLER
        // ---------------------------------------------

        private void HandleEnemyDied(Vector3 position, bool killedByPlayer)
        {
            if (_allWavesDone) return;
            if (GameEvents.IsGameOver) return;

            _remainingEnemies--;
            Debug.Log($"[WaveManager] Remaining enemies: {_remainingEnemies}");

            // Tüm düşmanlar öldü ve spawn bitti
            if (_remainingEnemies <= 0 && !_isSpawning)
            {
                StartCoroutine(CompleteWave());
            }
        }

        // ---------------------------------------------
        // Wave Flow
        // ---------------------------------------------

        private IEnumerator StartWave(int waveIndex)
        {
            WaveData wave = waves[waveIndex];

            if (hudController != null)
            {
                hudController.ShowWaveAnnouncement(wave.waveNumber, 5f);
            }

            yield return new WaitForSeconds(5f);

            if (GameEvents.IsGameOver) yield break;

            _remainingEnemies = wave.enemyCount;
            _isSpawning       = true;

            GameEvents.OnWaveStarted?.Invoke(wave.waveNumber);

            RespawnPowerUps();

            Debug.Log($"[WaveManager] Wave {wave.waveNumber} starting. Enemies: {wave.enemyCount}");

            // Observer: Report the wave number to HUD
            GameEvents.OnWaveCompleted?.Invoke(wave.waveNumber);

            // Spawn enemies one by one
            for (int i = 0; i < wave.enemyCount; i++)
            {
                if (GameEvents.IsGameOver) yield break;

                SpawnEnemy(wave);
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            _isSpawning = false;

            // Spawn is over, but the enemies might still be alive
            // HandleEnemyDied() will track the remaining enemies
            if (_remainingEnemies <= 0)
            {
                StartCoroutine(CompleteWave());
            }
        }

        private IEnumerator CompleteWave()
        {
            if (_allWavesDone) yield break;

            if (GameEvents.IsGameOver)
            {
                Debug.Log("[WaveManager] Core is destroyed.");
                yield break;
            }

            WaveData completedWave = waves[_currentWaveIndex];
            GameEvents.OnWaveCompleted?.Invoke(completedWave.waveNumber);
            Debug.Log($"[WaveManager] Wave {completedWave.waveNumber} is finished!");

            // Is there a next wave session?
            _currentWaveIndex++;

            if (_currentWaveIndex >= waves.Length)
            {
                // All waves is finished
                _allWavesDone = true;
                Debug.Log("[WaveManager] All Waves are FINISHED! YOU WON!");
                GameEvents.OnGameWon?.Invoke();
                yield break;
            }

            // A break between waves
            // float breakTime = completedWave.breakDuration;
            // Debug.Log($"[WaveManager] Next wave starting in {breakTime} seconds...");
            // yield return new WaitForSeconds(breakTime);
            yield return new WaitForSeconds(completedWave.breakDuration);

            if (!GameEvents.IsGameOver)
            {
                StartCoroutine(StartWave(_currentWaveIndex));
            }
        }

        // ---------------------------------------------
        // Enemy SPAWN
        // ---------------------------------------------

        private void SpawnEnemy(WaveData wave)
        {
            if (enemyPrefab == null)
            {
                Debug.LogError("[WaveManager] Enemy prefab is not assigned!");
                return;
            }

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("[WaveManager] There is no spawn point!");
                return;
            }

            // Select a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Produce enemy
            GameObject enemyObj = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            // -- STRATEGY PATTERN --
            // Set the strategy at runtime - EnemyController remains unchanged
            EnemyController controller = enemyObj.GetComponent<EnemyController>();
            if (controller != null)
            {

                switch (wave.strategyType)
                {
                    case WaveData.StrategyType.Direct:
                        controller.SetStrategy(new DirectMoveStrategy(enemySpeed));
                        break;

                    case WaveData.StrategyType.Zigzag:
                        controller.SetStrategy(new ZigzagMoveStrategy(enemySpeed));
                        break;

                    case WaveData.StrategyType.Mixed:
                        // Rastgele Direct veya Zigzag
                        if (Random.value > 0.5f)
                            controller.SetStrategy(new DirectMoveStrategy(enemySpeed));
                        else
                            controller.SetStrategy(new ZigzagMoveStrategy(enemySpeed));
                        break;
                }

                // Show the core target
                controller.SetTarget(coreTransform);
            }

            Debug.Log($"[WaveManager] Enemy spawned: {spawnPoint.name} | Strategy: {wave.strategyType}");
        }
        private void RespawnPowerUps()
        {
            if (powerUpSpawnPoints == null || powerUpSpawnPoints.Length < 2) return;

            // Eskilerini temizle
            if (_activePowerUpA != null) Destroy(_activePowerUpA);
            if (_activePowerUpB != null) Destroy(_activePowerUpB);

            // 2 rastgele farklı nokta seç
            int indexA = Random.Range(0, powerUpSpawnPoints.Length);
            int indexB;
            do { indexB = Random.Range(0, powerUpSpawnPoints.Length); }
            while (indexB == indexA);

            // Power-up'ları spawn et
            if (doubleDamagePrefab != null)
                _activePowerUpA = Instantiate(doubleDamagePrefab, powerUpSpawnPoints[indexA].position, Quaternion.identity);

            if (rapidFirePrefab != null)
                _activePowerUpB = Instantiate(rapidFirePrefab, powerUpSpawnPoints[indexB].position, Quaternion.identity);

            Debug.Log("[WaveManager] Power-up'lar yenilendi.");
        }
    }
}
