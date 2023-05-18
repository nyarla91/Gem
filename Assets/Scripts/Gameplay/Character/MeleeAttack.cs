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
    public class MeleeAttack : Transformable
    {
        public const string AttackState = "Attack";
        
        [SerializeField] private float _bufferWindow;
        [SerializeField] private float _comboDiscardTime;
        [SerializeField] private List<MeleeAttackMove> _combo;

        private int _attackPhase;
        private Timer _comboCooldown;
        private InputBuffer _attackBuffer;
        private Coroutine _attackCoroutine;

        private PlayerMovement _movement;

        private bool ComboAvailable => _attackPhase < _combo.Count;
        
        public MeleeAttackMove CurrentMove { get; private set; }

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
            Movable.VoluntaryVelocity = Vector3.zero;
            if (Pause.IsPaused || _attackPhase < 0 || ! ComboAvailable)
            {
                StateMachine.TryEnterState(AttackState);
                yield break;
            }
            
            CurrentMove = _combo[_attackPhase];
            _comboCooldown.Stop();

            for (int frame = 0; frame < CurrentMove.SwingFrames; frame++) 
            {
                if (Movement.WorldMoveInput.magnitude > 0)
                {
                    Transform.rotation = Quaternion.LookRotation(Movement.WorldMoveInput);
                }
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }

            Vector3 direction = Transform.forward;
            Movable.VoluntaryVelocity = CurrentMove.EvaluateThrustForPhase(0) * direction;
            CurrentMove.AttackArea.Content.Foreach(TryHitCollider);
            CurrentMove.AttackArea.Enter += TryHitCollider;
            
            for (int frame = 0; frame < CurrentMove.AttackFrames; frame++)
            {
                Movable.VoluntaryVelocity = CurrentMove.EvaluateThrustForPhase(frame / CurrentMove.AttackFrames) * direction;
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
            
            CurrentMove.AttackArea.Enter -= TryHitCollider;
            Movable.VoluntaryVelocity = Vector3.zero;

            for (int frame = 0; frame < CurrentMove.RecoverFrames; frame++)
            {
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
            
            StateMachine.TryExitState(AttackState);
        }

        private void TryHitCollider(Collider target)
        {
            if (CurrentMove == null || ! target.TryGetComponent(out Hitbox hitbx))
                return;
            Vector3 directionFrom = -Movable.VoluntaryVelocity;
            float force = Movable.VoluntaryVelocity.magnitude;
            hitbx.TakeHit(new Hit(1, directionFrom, force));
        }

        private void InterruptAttack()
        {
            _attackPhase++;
            _comboCooldown.Restart();
            _attackCoroutine?.Stop(this);
            CurrentMove = null;
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
    }

    [Serializable]
    public class MeleeAttackMove
    {
        [SerializeField] private OverlapTrigger _attackArea;
        [SerializeField] private int _swingFrames;
        [SerializeField] private int _attackFrames;
        [SerializeField] private int _recoverFrames;
        [SerializeField] private AnimationCurve _thrustOverAttack;
        [SerializeField] private float _thrustScale;
        [SerializeField] private int _animation;

        public OverlapTrigger AttackArea => _attackArea;
        public float SwingFrames => _swingFrames;
        public float AttackFrames => _attackFrames;
        public float RecoverFrames => _recoverFrames;
        public int Animation => _animation;

        public float EvaluateThrustForPhase(float phase)
        {
            phase = Mathf.Clamp(phase, 0, 1);
            return _thrustOverAttack.Evaluate(phase) * _thrustScale;
        }
    }
}