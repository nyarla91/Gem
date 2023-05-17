using System;
using UnityEngine;

namespace Gameplay.Character.Player.View
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;

        public Animator Animator => _animator ??= GetComponent<Animator>();
        
        public event Func<float> MovementSpeed;
        public event Func<int> State;
        public event Func<int> Attack;

        private void Start()
        {
            if (State == null)
                throw new UnassignedReferenceException("State is not binded");
            if (MovementSpeed == null)
                throw new UnassignedReferenceException("MovementSpeed is not binded");
            if (Attack == null)
                throw new UnassignedReferenceException("AttackAnimation is not binded");
        }

        private void FixedUpdate()
        {
            Animator.SetInteger("State", State?.Invoke() ?? 0);
            Animator.SetFloat("MovementSpeed", MovementSpeed?.Invoke() ?? 0);
            Animator.SetInteger("Attack", Attack?.Invoke() ?? 0);
        }
    }
}