using UnityEngine;

namespace Extensions
{
    public static class CustomExtensions
    {
        public static void ShowCanvasGroup(this CanvasGroup canvasGroup, bool state)
        {
            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}