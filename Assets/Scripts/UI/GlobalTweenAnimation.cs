using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public static class GlobalTweenAnimation
    {
        private const float AppearTime = 0.3f;

        public static void DOAppear(this Transform target, CanvasGroup canvasGroup)
        {
            target.DOComplete();
            target.DOScale(Vector3.one, AppearTime);
            canvasGroup.DOComplete();
            canvasGroup.DOFade(1, AppearTime);
        }

        public static void DODisappear(this Transform target, CanvasGroup canvasGroup)
        {
            target.DOComplete();
            target.DOScale(Vector3.one * 2, AppearTime);
            canvasGroup.DOComplete();
            canvasGroup.DOFade(0, AppearTime);
        }
    }
}