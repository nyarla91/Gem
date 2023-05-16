using System;
using Extentions;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static Extentions.Pause.Pause;

namespace Gameplay.Character.Player
{
    public class PlayerControls : MonoBehaviour
    {
        private GameplayActions _actions;

        [Inject] private IPauseInfo Pause { get; set; }
            
        public event Action DodgePressed;
        public event Action PrimaryAttackPressed;

        public Vector2 GetMoveVector() => _actions.Player.Move.ReadValue<Vector2>();
        
        private void Awake()
        {
            _actions = new GameplayActions();
            _actions.Enable();
            _actions.Player.Dodge.performed += InvokeDodge;
            _actions.Player.Attack.performed += InvokePrimaryAttack;
        }
        
        private void InvokeDodge(InputAction.CallbackContext _)
        {
            if (Pause.IsPaused)
                return;
            DodgePressed?.Invoke();
        }
        private void InvokePrimaryAttack(InputAction.CallbackContext _)
        {
            if (Pause.IsPaused)
                return;
            PrimaryAttackPressed?.Invoke();
        }

        private void OnDestroy()
        {
            _actions.Player.Dodge.performed -= InvokeDodge;
            _actions.Player.Attack.performed += InvokePrimaryAttack;
        }
    }
}