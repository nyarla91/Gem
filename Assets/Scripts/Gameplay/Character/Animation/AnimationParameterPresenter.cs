using System;
using UnityEngine;

namespace Gameplay.Character.Animation
{
    public abstract class AnimationParameterPresenter<TComponent> : MonoBehaviour where TComponent : Component
    {
        [SerializeField] private Animator _animator;

        private TComponent[] _components;

        private TComponent[] Components => _components ??= GetComponents<TComponent>();

        protected abstract void ApplyParameter(Animator animator, TComponent[] components);

        private void Update()
        {
            ApplyParameter(_animator, Components);
        }
    }
}