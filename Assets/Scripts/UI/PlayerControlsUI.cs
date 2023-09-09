using Extensions;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class PlayerControlsUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup buttonsCanvasGroup;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        public void Subscribe(UnityAction onLeftButton, UnityAction onRightButton)
        {
            leftButton.onClick.AddListener(onLeftButton);
            rightButton.onClick.AddListener(onRightButton);
            leftButton.onClick.AddListener(DisableControls);
            rightButton.onClick.AddListener(DisableControls);
        }

        public void UnSubscribe(UnityAction onLeftButton, UnityAction onRightButton)
        {
            leftButton.onClick.RemoveListener(onLeftButton);
            rightButton.onClick.RemoveListener(onRightButton);
            leftButton.onClick.RemoveListener(DisableControls);
            rightButton.onClick.RemoveListener(DisableControls);
        }

        private void DisableControls()
        {
            EnableControls(false);
        }

        public void EnableControls(bool state)
        {
            buttonsCanvasGroup.interactable = state;
        }

        public void ShowControls(bool state)
        {
            buttonsCanvasGroup.ShowCanvasGroup(state);
        }
    }
}