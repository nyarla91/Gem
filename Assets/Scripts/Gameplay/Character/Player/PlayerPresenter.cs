using System;
using Extentions;
using Gameplay.Character.Player.View;
using UnityEngine;

namespace Gameplay.Character.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerPresenter : Transformable, IPlayerTransformInfo
    {
        [SerializeField] private PlayerControls _controls;

        private Movable _movable;
        private StateMachine _stateMachine;
        private PlayerMovement _movement;
        private MeleeAttack _meleeAttack;

        public Movable Movable => _movable ??= GetComponent<Movable>();
        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        public MeleeAttack MeleeAttack => _meleeAttack ??= GetComponent<MeleeAttack>();

        public Vector3 Position => Transform.position;
        public float YRotation => Transform.rotation.eulerAngles.y;
        
        private void Awake()
        {
            _controls.DodgePressed += Movement.TryDodge;
            _controls.PrimaryAttackPressed += () => MeleeAttack.TryAttack(0);

            Movement.MoveInput += () => _controls.MoveVector;
            
            MeleeAttack.SwingAttackDirection += () => Movement.WorldMoveInput;
        }

    }
}