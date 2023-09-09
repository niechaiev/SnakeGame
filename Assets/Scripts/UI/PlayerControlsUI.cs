using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class PlayerControlsUI : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        private void OnEnable()
        {
            leftButton.onClick.AddListener(DisableControls);
            rightButton.onClick.AddListener(DisableControls);
        }

        public void Subscribe(UnityAction onLeftButton, UnityAction onRightButton)
        {
            leftButton.onClick.AddListener(onLeftButton);
            rightButton.onClick.AddListener(onRightButton);
        }

        public void UnSubscribe(UnityAction onLeftButton, UnityAction onRightButton)
        {
            leftButton.onClick.RemoveListener(onLeftButton);
            rightButton.onClick.RemoveListener(onRightButton);
        }

        private void DisableControls()
        {
            EnableControls(false);
        }

        public void EnableControls(bool state)
        {
            leftButton.enabled = state;
            rightButton.enabled = state;
        }

        private void OnDisable()
        {
            leftButton.onClick.RemoveListener(DisableControls);
            rightButton.onClick.RemoveListener(DisableControls);
        }
    }
}