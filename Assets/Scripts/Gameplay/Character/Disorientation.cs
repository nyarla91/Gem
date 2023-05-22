using System;
using System.Collections;
using Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Disorientation))]
    public class Disorientation : MonoBehaviour
    {
        public const string StaggerState = "Stagger";
        public const string StunState = "Stun";
        public const float StaggerDuration = 0.3f;

        private Timer _disorientationExit;

        private StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        
        [Inject] private IPauseInfo Pause { get; set; }

        public void Stagger()
        {
            if (StateMachine.TryEnterState(StaggerState))
            {
                _disorientationExit.Length = StaggerDuration;
                _disorientationExit.Restart();
            }
        }

        private void InterruptExit()
        {
            _disorientationExit.Stop();
        }

        private void Awake()
        {
            _disorientationExit = new Timer(this, StaggerDuration, Pause);
            _disorientationExit.Expired += () =>
            {
                StateMachine.TryExitState(StaggerState);
                StateMachine.TryExitState(StunState);
            };
            StateMachine.GetState(StaggerState).Exit += InterruptExit;
            StateMachine.GetState(StunState).Exit += InterruptExit;
        }
    }
}