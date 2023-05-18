using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Disorientation))]
    public class Disorientation : MonoBehaviour
    {
        public const string StaggerState = "Stagger";
        public const string StunState = "Stun";
        private const int StaggerFtames = 15;

        private Coroutine _exitCoroutine;

        private StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        
        public void Stagger()
        {
            if (StateMachine.TryEnterState(StaggerState))
                _exitCoroutine = StartCoroutine(ExitDisorientation(StaggerState));
        }

        private IEnumerator ExitDisorientation(string state)
        {
            yield break;
        }

        private void Awake()
        {
            
        }
    }
}