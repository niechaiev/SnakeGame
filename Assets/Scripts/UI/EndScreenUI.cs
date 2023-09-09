using System;
using Game;
using TMPro;
using Unity.VisualScripting;
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
            endScreen.alpha = state ? 1 : 0;
            endScreen.interactable = state;
            endScreen.blocksRaycasts = state;
        }

        public void HideEndScreen()
        {
            ShowCanvasGroup(false);
        }
    }
}