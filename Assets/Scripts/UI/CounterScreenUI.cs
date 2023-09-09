using Extensions;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CounterScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup counterCanvasGroup;
        
        [SerializeField] private TMP_Text counterText;

        public void UpdateCount(int snakeSize)
        {
            counterText.text = $"Snake size: {snakeSize}";
        }

        public void ShowCounter(bool state)
        {
            counterCanvasGroup.ShowCanvasGroup(state);
        }
        
    }
}