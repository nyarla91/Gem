using DG.Tweening;
using Extentions;
using JetBrains.Annotations;
using UnityEngine;

namespace UI
{
    public class SelectionBox : Transformable
    {
        public void Hide(RectTransform canvas)
        {
            RectTransform.SetParent(canvas);
            gameObject.SetActive(false);
        }
        
        public void MoveTo([NotNull] RectTransform target)
        {
            gameObject.SetActive( true);
            RectTransform.SetParent(target);
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.sizeDelta = Vector2.zero;
            RectTransform.anchoredPosition = Vector2.zero;
            RectTransform.localScale = Vector3.one;
            RectTransform.DOComplete();
            RectTransform.DOShakeScale(0.1f, new Vector2(0.1f, 0.1f));
        }
    }
}