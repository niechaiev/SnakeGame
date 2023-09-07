using System;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerControlsUI : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private GameContext gameContext;

        private void OnEnable()
        {
            leftButton.onClick.AddListener(gameContext.Game.TurnLeft);
            rightButton.onClick.AddListener(gameContext.Game.TurnRight);
        }

        private void OnDisable()
        {
            leftButton.onClick.RemoveListener(gameContext.Game.TurnLeft);
            rightButton.onClick.RemoveListener(gameContext.Game.TurnRight);
        }
    }
}