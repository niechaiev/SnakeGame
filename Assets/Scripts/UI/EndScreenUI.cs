using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup endScreen;
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Color defeatColor;
        [SerializeField] private Color victoryColor;
        [SerializeField] private Image backgroundImage;

        public void Subscribe(UnityAction onRestartButtonPressed)
        {
            restartButton.onClick.AddListener(onRestartButtonPressed);
        }

        public void UnSubscribe(UnityAction onRestartButtonPressed)
        {
            restartButton.onClick.RemoveListener(onRestartButtonPressed);
        }

        public void ShowDefeatScreen()
        {
            tmpText.text = "DEFEAT";
            endScreen.ShowCanvasGroup(true);
            backgroundImage.color = defeatColor;
        }

        public void ShowVictoryScreen()
        {
            tmpText.text = "VICTORY";
            endScreen.ShowCanvasGroup(true);
            backgroundImage.color = victoryColor;
        }

        public void HideEndScreen()
        {
            endScreen.ShowCanvasGroup(false);
        }
    }
}