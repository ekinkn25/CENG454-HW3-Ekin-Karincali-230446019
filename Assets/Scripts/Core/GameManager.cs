using UnityEngine;
using UnityEngine.SceneManagement;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        // ---------------------------------------------
        // OBSERVER — SUBSCRIBE / UNSUBSCRIBE
        // ---------------------------------------------

        private void OnEnable()
        {
            GameEvents.OnCoreDead += HandleLose;
            GameEvents.OnGameWon  += HandleWin;
        }

        private void OnDisable()
        {
            GameEvents.OnCoreDead -= HandleLose;
            GameEvents.OnGameWon  -= HandleWin;
        }

        private void Start()
        {
            // Clear all events at the start of the scene
            // Reset subscribers left over from the previous scene
            // GameEvents.ResetAllEvents();

            // Start with panels are off
            if (winPanel  != null) winPanel.SetActive(false);
            if (losePanel != null) losePanel.SetActive(false);

            // Reset the game speed - it may remain at 0 after a restart            
            Time.timeScale = 1f;

            Debug.Log("[GameManager] Game started.");
        }

        // ---------------------------------------------
        // WİN
        // ---------------------------------------------

        private void HandleWin()
        {
            Debug.Log("[GameManager] GAME WON!");

            DisableAllEnemies();

            if (winPanel != null)
                winPanel.SetActive(true);

            // stop the game
            Time.timeScale = 0f;
        }

        // ---------------------------------------------
        // LOSE
        // ---------------------------------------------

        private void HandleLose()
        {
            Debug.Log("[GameManager] GAME LOST!");

            if (losePanel != null)
                losePanel.SetActive(true);

            Time.timeScale = 0f;
        }

        // ---------------------------------------------
        // RESTART
        // ---------------------------------------------

        public void RestartGame()
        {
            // Reset the game speed - it may remain at 0 after a restart
            Time.timeScale = 1f;

            // Clear all events - prevention from ghost subscriber
            GameEvents.ResetAllEvents();

            // Reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Debug.Log("[GameManager] Game restarted.");
        }

        // ---------------------------------------------
        // HEALPER
        // ---------------------------------------------

        private void DisableAllEnemies()
        {
            // Find all active enemies and disable them
            Enemies.EnemyHealth[] enemies =
                FindObjectsByType<Enemies.EnemyHealth>(FindObjectsSortMode.None);

            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }

            Debug.Log($"[GameManager] {enemies.Length} enemy disabled.");
        }
    }
}