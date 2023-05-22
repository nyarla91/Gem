using System;
using UnityEngine;

namespace Gameplay.Character
{
    [RequireComponent(typeof(TrailRenderer))]
    public class MeleeAttackTrail : MonoBehaviour
    {
        [SerializeField] private MeleeAttack _meleeAttack;

        private TrailRenderer _trailRenderer;
        private TrailRenderer TrailRenderer => _trailRenderer ??= GetComponent<TrailRenderer>();

        private void Update()
        {
            TrailRenderer.emitting = _meleeAttack.AttackStep == AttackStep.Attack;
        }
    }
}