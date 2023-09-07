using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup endScreen; 
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private Button restartButton;
        [SerializeField] private GameContext gameContext;
        

        private void Start()
        {
            restartButton.onClick.AddListener(RestartButtonPressed);
        }

        private void RestartButtonPressed()
        {
            gameContext.RestartGame();
        }
        
        public void ShowGameOverScreen(bool state)
        {
            tmpText.text = "DEFEAT";
            endScreen.enabled = state;
            
        }

        public void ShowVictoryScreen(bool state)
        {
            tmpText.text = "VICTORY";
            endScreen.enabled = state;
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(RestartButtonPressed);
        }
    }
}