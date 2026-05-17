using System.Collections;
using UnityEngine;
using TMPro;
using CoreBreach.Patterns.Observer;

namespace CoreBreach.UI
{
    public class WaveTransitionUI : MonoBehaviour
    {
        [SerializeField] private GameObject      transitionPanel;
        [SerializeField] private TextMeshProUGUI transitionText;
        [SerializeField] private float           displayDuration = 2f;

        private void OnEnable()
        {
            GameEvents.OnWaveStarted += HandleWaveStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnWaveStarted -= HandleWaveStarted;
        }

        private void HandleWaveStarted(int waveNumber)
        {
            StartCoroutine(ShowTransition(waveNumber));
        }

        private IEnumerator ShowTransition(int waveNumber)
        {
            if (transitionPanel == null) yield break;

            transitionText.text = $"WAVE {waveNumber}";
            transitionPanel.SetActive(true);

            // 2 saniye göster
            yield return new WaitForSeconds(displayDuration);

            transitionPanel.SetActive(false);
        }
    }
}