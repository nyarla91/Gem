using System;
using UnityEngine;

namespace Extentions
{
    public class LazyGetComponent<T> : Transformable where T : Component
    {
        private T _lazy;

        protected virtual T Lazy => _lazy ??= GetComponent<T>();
    }
}