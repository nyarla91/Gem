using System;
using Gameplay.Character.Player.View;
using UnityEngine;

namespace Gameplay.Character.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerControls _controls;
        [SerializeField] private PlayerAnimation _animation;

        private Movable _movable;
        private StateMachine _stateMachine;
        private PlayerMovement _movement;
        private MeleeAttack _meleeAttack;

        public Movable Movable => _movable ??= GetComponent<Movable>();
        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        public MeleeAttack MeleeAttack => _meleeAttack ??= GetComponent<MeleeAttack>();

        private void Awake()
        {
            Movement.MoveInput += () => _controls.MoveVector;
            _controls.DodgePressed += Movement.TryDodge;
            _controls.PrimaryAttackPressed += MeleeAttack.TryAttack;
            _animation.State += () => StateMachine.CurrentState.ID;
            _animation.MovementSpeed += () => Movable.Velocity.magnitude;
        }
    }
}