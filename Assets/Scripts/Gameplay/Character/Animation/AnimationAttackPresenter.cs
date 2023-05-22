using UnityEngine;

namespace Gameplay.Character.Animation
{
    [RequireComponent(typeof(MeleeAttack))]
    public class AnimationAttackPresenter : AnimationParameterPresenter<MeleeAttack>
    {
        protected override void ApplyParameter(Animator animator, MeleeAttack[] components)
        {
            animator.SetInteger("Attack", components[0].CurrentMove?.Animation ?? 0);
        }
    }
}