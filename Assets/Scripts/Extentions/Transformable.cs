using System;
using UnityEngine;

namespace Extentions
{
    public abstract class Transformable : MonoBehaviour
    {
        private Transform _transform;
        [Obsolete] public new Transform transform => _transform ??= gameObject.transform;
        public Transform Transform => transform;

        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
    }
}