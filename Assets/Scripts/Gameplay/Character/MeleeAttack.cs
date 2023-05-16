using System;
using System.Collections;
using System.Collections.Generic;
using Extentions;
using Gameplay.Character.Player;
using Input;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(Movable))]
    public class MeleeAttack : MonoBehaviour
    {
        public const string AttackState = "Attack";
        
        [SerializeField] private float _bufferWindow;
        [SerializeField] private float _comboDiscardTime;
        [SerializeField] private List<MeleeAttackMove> _combo;

        private Vector3 _direction;
        private int _attackPhase;
        private Timer _comboCooldown;
        private InputBuffer _attackBuffer;
        private Coroutine _attackCoroutine;

        private PlayerMovement _movement;

        private bool ComboAvailable => _attackPhase < _combo.Count;

        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        private StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        private Movable _movable;
        public Movable Movable => _movable ??= GetComponent<Movable>();
        
        [Inject] private IPauseInfo Pause { get; set; }
        
        public void TryAttack()
        {
            _attackBuffer.SendInput();
        }

        private void StartAttack()
        {
            _attackCoroutine = StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            Movable.Velocity = Vector3.zero;
            if (Pause.IsPaused || _attackPhase < 0 || ! ComboAvailable)
            {
                StateMachine.TryEnterState(AttackState);
                yield break;
            }
            
            MeleeAttackMove move = _combo[_attackPhase];

            _comboCooldown.Stop();
            yield return new PausableWaitForSeconds(this, Pause, move.SwingTime);
            Vector3 direction = _direction;
            Movable.Velocity = direction * move.PushForce / 0.1f;
            yield return new PausableWaitForSeconds(this, Pause, 0.1f);
            Movable.Velocity = Vector3.zero;
            yield return new PausableWaitForSeconds(this, Pause, move.RestoreTime);
            StateMachine.TryExitState(AttackState);
        }

        private void InterruptAttack()
        {
            _attackPhase++;
            _comboCooldown.Restart();
            _attackCoroutine?.Stop(this);
        }

        private void Start()
        {
            StateMachine.GetState(AttackState).Exit += InterruptAttack;
            
            _attackBuffer = new InputBuffer(this, _bufferWindow);
            _attackBuffer.Performed += StartAttack;
            _attackBuffer.PerformCondition += () => ComboAvailable && StateMachine.TryEnterState(AttackState);

            _comboCooldown = new Timer(this, _comboDiscardTime, Pause);
            _comboCooldown.Expired += () => _attackPhase = 0;
        }

        private void FixedUpdate()
        {
            if (Movement.WorldMoveInput.magnitude > 0)
                _direction = Movement.WorldMoveInput.normalized;
        }
    }

    [Serializable]
    public class MeleeAttackMove
    {
        [SerializeField] private OverlapTrigger _attackArea;
        [SerializeField] private float _swingTime;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _restoreTime;

        public OverlapTrigger AttackArea => _attackArea;
        public float SwingTime => _swingTime;
        public float PushForce => _pushForce;
        public float RestoreTime => _restoreTime;
    }
}