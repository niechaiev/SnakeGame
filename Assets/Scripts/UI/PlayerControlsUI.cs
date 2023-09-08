using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class PlayerControlsUI : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [FormerlySerializedAs("gameContext")] [SerializeField] private GameVisualizer gameVisualizer;

        private void OnEnable()
        {
            leftButton.onClick.AddListener(gameVisualizer.Game.TurnLeft);
            rightButton.onClick.AddListener(gameVisualizer.Game.TurnRight);
        }

        private void OnDisable()
        {
            leftButton.onClick.RemoveListener(gameVisualizer.Game.TurnLeft);
            rightButton.onClick.RemoveListener(gameVisualizer.Game.TurnRight);
        }
    }
}