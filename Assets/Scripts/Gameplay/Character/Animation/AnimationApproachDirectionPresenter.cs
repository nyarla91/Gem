using Gameplay.Character.Enemy;
using UnityEngine;

namespace Gameplay.Character.Animation
{
    [RequireComponent(typeof(EnemyMovement))]
    public class AnimationApproachDirectionPresenter : AnimationParameterPresenter<EnemyMovement>
    {
        protected override void ApplyParameter(Animator animator, EnemyMovement[] components)
        {
            animator.SetInteger("ApproachDirection", components[0].ApproachDirection);
        }
    }
}