using System;
using System.Collections;
using Extentions;
using Input;
using UnityEngine;
using Zenject;

namespace Gameplay.Character.Player
{
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(StateMachine))]
    public class PlayerMovement : Transformable
    {
        public const string DodgeState = "Dodge";
        
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _dodgeBufferWindow;
        [SerializeField] private AnimationCurve _dodgeSpeedCurve;
        [SerializeField] private float _dodgeSpeedScale;
        [SerializeField] private int _dodgeFrames;
        [SerializeField] private float _dodgeCooldown;

        private Vector3 _velocity;
        private InputBuffer _dodgeBuffer;
        private Timer _dodgeRestoration;
        private Coroutine _dodgeCoroutine;

        private Movable _movable;
        public Movable Movable => _movable ??= GetComponent<Movable>();
        private StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        
        public event Func<Vector2> MoveInput;

        private bool DodgePressAllowed => WorldMoveInput.magnitude > 0;

        public Vector3 WorldMoveInput
        {
            get
            {
                if (MoveInput == null)
                    throw new UnassignedReferenceException("MoveInput is not binded");
                Vector2 screenInput = MoveInput.Invoke();
                return MainCamera.Transform.forward.WithY(0).normalized * screenInput.y + MainCamera.Transform.right * screenInput.x;
            }
        }

        [Inject] private MainCamera MainCamera { get; set; }
        [Inject] private IPauseInfo Pause { get; set; }


        public void TryDodge()
        {
            if ( ! DodgePressAllowed)
                return;
            _dodgeBuffer.SendInput();
        }

        private void StartDodge() => _dodgeCoroutine = StartCoroutine(Dodge());
        private IEnumerator Dodge()
        {
            Vector3 direction = WorldMoveInput.normalized;
            
            Transform.rotation = Quaternion.LookRotation(direction);
            for (float frame = 0; frame < _dodgeFrames; frame++)
            {
                Movable.Velocity = _dodgeSpeedCurve.Evaluate(frame / _dodgeFrames) * _dodgeSpeedScale * direction;
                if (Pause.IsPaused)
                    yield return new WaitUntil(() => Pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
            StateMachine.TryExitState(DodgeState);
        }

        private void InterruptDodge()
        {
            _dodgeCoroutine?.Stop(this);
            _dodgeRestoration.Restart();
        }
        
        private void Start()
        {
            StateMachine.GetState(DodgeState).Exit += InterruptDodge;
            
            _dodgeBuffer = new InputBuffer(this, _dodgeBufferWindow);
            _dodgeBuffer.PerformCondition += () => DodgePressAllowed && ! _dodgeRestoration.IsOn && StateMachine.TryEnterState(DodgeState);
            _dodgeBuffer.Performed += StartDodge;
            
            _dodgeRestoration = new Timer(this, _dodgeCooldown, Pause);
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        private void Move( float deltaTime)
        {
            if (Pause.IsPaused || StateMachine.IsCurrentStateNoneOf(StateMachine.Regular))
                return;
            Vector3 targetVelocity = WorldMoveInput;
            targetVelocity *= _maxSpeed;
            _velocity = Vector3.MoveTowards(_velocity, targetVelocity, _maxSpeed * _acceleration * deltaTime);
            Movable.Velocity = _velocity;
            if (_velocity.magnitude > 0)
                Transform.rotation = Quaternion.LookRotation(_velocity);
        }
    }
}