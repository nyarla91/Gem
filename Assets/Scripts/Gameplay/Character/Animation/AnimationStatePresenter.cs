using System;
using UnityEngine;

namespace Gameplay.Character.Animation
{
    [RequireComponent(typeof(StateMachine))]
    public class AnimationStatePresenter : AnimationParameterPresenter<StateMachine>
    {
        protected override void ApplyParameter(Animator animator, StateMachine[] components)
            => animator.SetInteger("State", components[0].CurrentState.ID);
    }
}