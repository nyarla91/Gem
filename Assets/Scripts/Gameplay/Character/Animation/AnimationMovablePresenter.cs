using UnityEngine;

namespace Gameplay.Character.Animation
{
    [RequireComponent(typeof(Movable))]
    public class AnimationMovablePresenter : AnimationParameterPresenter<Movable>
    {
        protected override void ApplyParameter(Animator animator, Movable[] components)
            => animator.SetFloat("MovementSpeed", components[0].VoluntaryVelocity.magnitude);
    }
}