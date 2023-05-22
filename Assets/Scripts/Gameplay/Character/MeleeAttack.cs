using System;
using System.Collections;
using Extentions;
using Input;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Sight))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(Movable))]
    public class MeleeAttack : Transformable
    {
        public const string AttackState = "Attack";
        
        [SerializeField] private float _bufferWindow;
        [SerializeField] private float _comboDiscardTime;
        [SerializeField] private MeleeCombo[] _combo;

        private int _currentCombo;
        private int _attackPhase;
        private Timer _comboCooldown;
        private InputBuffer[] _attackBuffers;
        private Coroutine _attackCoroutine;

        public AttackStep AttackStep { get; private set; } = AttackStep.None;
        public MeleeAttackMove CurrentMove { get; private set; }

        private Sight _sight;
        private Movable _movable;
        private StateMachine _stateMachine;

        private Sight Sight => _sight ??= GetComponent<Sight>();
        private Movable Movable => _movable ??= GetComponent<Movable>();
        private StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();

        public event Func<Vector3> SwingAttackDirection; 
        
        [Inject] private IPauseInfo Pause { get; set; }
        
        public void TryAttack(int combo)
        {
            if (combo < 0 || combo >= _combo.Length)
                throw new IndexOutOfRangeException($"{gameObject} has no {combo} combo");
            _attackBuffers[combo].SendInput();
        }

        private void StartAttack(int combo)
        {
            _attackCoroutine = StartCoroutine(Attack(combo));
        }

        private IEnumerator Attack(int combo)
        {
            Movable.VoluntaryVelocity = Vector3.zero;
            if (Pause.IsPaused || _attackPhase < 0 || ! IsNextComboMoveAvailable(combo))
            {
                StateMachine.TryEnterState(AttackState);
                yield break;
            }

            if (_currentCombo != combo)
            {
                _attackPhase = 0;
                _currentCombo = combo;
            }
            CurrentMove = _combo[_currentCombo].GetMove(_attackPhase);
            _comboCooldown.Stop();

            AttackStep = AttackStep.Swing;
            for (int frame = 0; frame < CurrentMove.SwingFrames; frame++) 
            {
                if (SwingAttackDirection != null)
                    Sight.RotateTowardsDirection(SwingAttackDirection.Invoke(), true);
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }

            Vector3 direction = Transform.forward;
            Movable.VoluntaryVelocity = CurrentMove.EvaluateThrustForPhase(0) * direction;
            CurrentMove.AttackArea.Content.Foreach(TryHitCollider);
            CurrentMove.AttackArea.Enter += TryHitCollider;
            
            AttackStep = AttackStep.Attack;
            for (int frame = 0; frame < CurrentMove.AttackFrames; frame++)
            {
                Movable.VoluntaryVelocity = CurrentMove.EvaluateThrustForPhase(frame / CurrentMove.AttackFrames) * direction;
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
            
            CurrentMove.AttackArea.Enter -= TryHitCollider;
            Movable.VoluntaryVelocity = Vector3.zero;

            AttackStep = AttackStep.Recovery;
            for (int frame = 0; frame < CurrentMove.RecoverFrames; frame++)
            {
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
            
            StateMachine.TryExitState(AttackState);
        }
        
        private bool IsNextComboMoveAvailable(int combo)
        {
            print($"{combo} {_currentCombo} {_attackPhase} {_currentCombo != combo || _attackPhase < _combo[_currentCombo].Phases}");
            return _currentCombo != combo || _attackPhase < _combo[_currentCombo].Phases;
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
            AttackStep = AttackStep.None;
            _attackPhase++;
            _comboCooldown.Restart();
            _attackCoroutine?.Stop(this);
            CurrentMove = null;
        }

        private void Start()
        {
            StateMachine.GetState(AttackState).Exit += InterruptAttack;

            _attackBuffers = new InputBuffer[_combo.Length];
            for (int i = 0; i < _combo.Length; i++)
            {
                int combo = i;
                _attackBuffers[i] = new InputBuffer(this, _bufferWindow);
                _attackBuffers[i].Performed += () => StartAttack(combo);
                _attackBuffers[i].PerformCondition += () => IsNextComboMoveAvailable(combo) && StateMachine.TryEnterState(AttackState);
            }

            _comboCooldown = new Timer(this, _comboDiscardTime, Pause);
            _comboCooldown.Expired += () => _attackPhase = 0;
        }
    }

    public enum AttackStep
    {
        None,
        Swing,
        Attack,
        Recovery,
    }

    [Serializable]
    public class MeleeCombo
    {
        [SerializeField] private MeleeAttackMove[] _moves;

        public int Phases => _moves.Length;
        
        public MeleeAttackMove GetMove(int phase)
        {
            if (phase < 0 || phase >= _moves.Length)
                throw new IndexOutOfRangeException($"{this} melee combo has no phase {phase}");
            return _moves[phase];
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