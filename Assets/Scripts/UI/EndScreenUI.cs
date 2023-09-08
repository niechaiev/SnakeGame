using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup endScreen; 
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private Button restartButton;
        [SerializeField] private GameVisualizer gameVisualizer;
        [SerializeField] private Color defeatColor;
        [SerializeField] private Color victoryColor;
        [SerializeField] private Image backgroundImage;
        
        private void Start()
        {
            restartButton.onClick.AddListener(RestartButtonPressed);
        }

        private void RestartButtonPressed()
        {
            gameVisualizer.RestartGame();
        }
        
        public void ShowDefeatScreen()
        {
            tmpText.text = "DEFEAT";
            ShowCanvasGroup(true);
            backgroundImage.color = defeatColor;
        }

        public void ShowVictoryScreen()
        {
            tmpText.text = "VICTORY";
            ShowCanvasGroup(true);
            backgroundImage.color = victoryColor;
        }
        private void ShowCanvasGroup(bool state)
        {
            endScreen.alpha = state ? 1: 0;
            endScreen.interactable = state;
            endScreen.blocksRaycasts = state;
        }
        public void HideEndScreen()
        {
            ShowCanvasGroup(false);
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(RestartButtonPressed);
        }
    }
}