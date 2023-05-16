using System;
using UnityEngine;

namespace Gameplay.Character.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerControls _controls;

        private PlayerMovement _movement;
        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        private MeleeAttack _meleeAttack;

        public MeleeAttack MeleeAttack => _meleeAttack ??= GetComponent<MeleeAttack>();

        private void Awake()
        {
            Movement.MoveInput += _controls.GetMoveVector;
            _controls.DodgePressed += Movement.TryDodge;
            _controls.PrimaryAttackPressed += MeleeAttack.TryAttack;
        }
    }
}