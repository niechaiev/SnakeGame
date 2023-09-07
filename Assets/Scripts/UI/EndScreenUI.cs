using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup gameOverScreen;
        [SerializeField] private CanvasGroup victoryScreen;
        [SerializeField] private Button restartButton;
        [SerializeField] private Field field;
        

        private void Start()
        {
            restartButton.onClick.AddListener(RestartButtonPressed);
        }

        private void RestartButtonPressed()
        {
            field.InitializeField();
        }
        
        public void ShowGameOverScreen(bool state)
        {
            gameOverScreen.enabled = state;
        }

        public void ShowVictoryScreen(bool state)
        {
            victoryScreen.enabled = state;
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(RestartButtonPressed);
        }
    }
}