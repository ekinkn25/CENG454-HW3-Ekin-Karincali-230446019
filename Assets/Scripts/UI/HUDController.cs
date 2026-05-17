using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("Core Health")]
        [SerializeField] private Slider healthBar;

        [Header("Wave Info")]
        [SerializeField] private TextMeshProUGUI waveText;

        [Header("Score")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [Header("Wave Announcement")]
        [SerializeField] private TextMeshProUGUI waveAnnouncementText;

        private int _currentScore = 0;

        //subscribe//unsubscribed
        //we use OnEnable and OnDisable because when an object is disabled, OnDestroy is not run but OnDisable will run. this will prevents ghost subscribber bug
        //HW Debug Report #003

        private void OnEnable()
        {
            GameEvents.OnCoreHealthChanged += HandleCoreHealtChanged;
            GameEvents.OnWaveCompleted += HandleWaveCompleted;
            GameEvents.OnEnemyDied += HandleEnemyDied;
            GameEvents.OnWaveStarted += HandleWaveStarted;
            Debug.Log("[HUDController] is subscribed to GameEvents.");
        }

        private void OnDisable()
        {
            GameEvents.OnCoreHealthChanged -= HandleCoreHealtChanged;
            GameEvents.OnWaveCompleted -= HandleWaveCompleted;
            GameEvents.OnEnemyDied -= HandleEnemyDied;
            GameEvents.OnWaveStarted -= HandleWaveStarted;
            Debug.Log("[HUDController] is unsubscribed to GameEvents.");
        }
        private void Start()
        {
            UpdateHealthBar(1f);
            UpdateWaveText(1);
            UpdateScoreText();
        }

        //event handlers: it will cal when OnCoreHealthChanged is triggered.
        //CoreHealth dont know this method directly just know frome events
        private void HandleCoreHealtChanged(float currentHealth, float maxHealth)
        {
            float ratio = currentHealth / maxHealth;
            UpdateHealthBar(ratio);
            Debug.Log($"[HUDController] life bar is updated: {ratio *100f:F0}%");
        }

        //it will call when OnWavecompleted is triggered
        // //TODO: WaveMAnager yazılında bu event otomatik gelecek
        private void HandleWaveCompleted(int waveNumber)
        {
            UpdateWaveText(waveNumber +1);
            Debug.Log($"[HUDController] Wave is updated: {waveNumber +1}");
        }

        //it will called when OnEnemyDied is triggered
        //HuD dont use Vector3 posiition parameter but it has to comply the event signature
        private void HandleEnemyDied(Vector3 position, bool killedByPlayer)
        {
            if (!killedByPlayer) return;
            _currentScore += 10;
            UpdateScoreText();
            Debug.Log($"[Hudcontroller] Score is updated: {_currentScore}");
        }


        //UI updating methods
        private void UpdateHealthBar(float ratio)
        {
            if (healthBar == null)
            {
                Debug.LogWarning("[Hudcontroller] HealthBar is not assigned!!!");
                return;
            }
            healthBar.value = ratio;
        }
        private void UpdateWaveText(int waveNumber)
        {
            if (waveText == null)
            {
                Debug.LogWarning("[HUDController] WaveText atanmamış!");
                return;
            }
            waveText.text = $"Wave {waveNumber}";
        }
        private void UpdateScoreText()
        {
            if (scoreText == null)
            {
                Debug.LogWarning("[HUDController] ScoreText atanmamış!");
                return;
            }
            scoreText.text = $"Score: {_currentScore}";
        }
        private void HandleWaveStarted(int waveNumber)
        {
            UpdateWaveText(waveNumber);
            Debug.Log($"[HUDController] Wave güncellendi: {waveNumber}");
        }
        public void ShowWaveAnnouncement(int waveNumber, float duration = 3f)
        {
            StartCoroutine(WaveAnnouncementCoroutine(waveNumber, duration));
        }

        private IEnumerator WaveAnnouncementCoroutine(int waveNumber, float duration)
        {
            if (waveAnnouncementText == null) yield break;

            waveAnnouncementText.text    = $"Wave {waveNumber} Başlıyor!";
            waveAnnouncementText.gameObject.SetActive(true);

            // Geri sayım göster
            float timer = duration;
            while (timer > 0f)
            {
                waveAnnouncementText.text = $"Wave {waveNumber}\n{Mathf.CeilToInt(timer)} saniye sonra başlıyor...";
                timer -= Time.deltaTime;
                yield return null;
            }

            waveAnnouncementText.gameObject.SetActive(false);
        }
    }
}
