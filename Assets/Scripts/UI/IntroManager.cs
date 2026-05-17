using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreBreach.UI
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private string gameSceneName = "GameScene";

        public void StartGame()
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}