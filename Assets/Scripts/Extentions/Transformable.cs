using System;
using UnityEngine;

namespace Extentions
{
    public abstract class Transformable : MonoBehaviour
    {
        private Transform _transform;
        [Obsolete] public new Transform transform => Transform;
        public Transform Transform => _transform ??= gameObject.transform;

        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
    }
}