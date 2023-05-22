using System;
using System.Collections;
using Extentions;
using UnityEngine;

namespace Gameplay.Character.Enemy
{
    [RequireComponent(typeof(MeleeAttack))]
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float _agressionThreshold;
        [SerializeField] private EnemyAgressionPattern[] _agressivePatterns;

        private Coroutine _agressionCoroutine;
        private float _aggression;

        private MeleeAttack _meleeAttack;
        private StateMachine _stateMachine;
        private EnemyMovement _Movement;
        private Movable _movable;

        private MeleeAttack MeleeAttack => _meleeAttack ??= GetComponent<MeleeAttack>();

        public StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();
        private EnemyMovement Movement => _Movement ??= GetComponent<EnemyMovement>();
        public Movable Movable => _movable ??= GetComponent<Movable>();
        
        public EnemyAgressionPattern CurrentPattern { get; private set; }

        private void Awake()
        {
            MeleeAttack.SwingAttackDirection += () => Movement.DirectionToPlayer;
        }

        private void FixedUpdate()
        {
            AdvanceAgression();
        }

        private void AdvanceAgression()
        {
            if (CurrentPattern != null)
                return;
            
            _aggression += Time.fixedDeltaTime;
            
            if (!(_aggression >= _agressionThreshold))
                return;
            
            EnemyAgressionPattern pattern = _agressivePatterns.PickRandomElement();
            StartCoroutine(PerformMove(pattern));
        }

        private IEnumerator PerformMove(EnemyAgressionPattern pattern)
        {
            Movement.CurrentPattern = pattern.Movement;
            Movable.SpeedMultiplier = pattern.SpeedMultipier;
            yield return new WaitUntil(() => Movement.IsWithinDesiredRange);
            MeleeAttack.TryAttack(pattern.MeleeCombo);
            yield return new WaitUntil(() => StateMachine.IsCurrentStateOneOf(MeleeAttack.AttackState));
            yield return new WaitUntil(() => StateMachine.IsCurrentStateOneOf(StateMachine.Regular));
            InterruptPattern();
        }

        private void InterruptPattern()
        {
            _agressionCoroutine?.Stop(this);
            _aggression = 0;
            Movement.RevertDefaultPattern();
            Movable.SpeedMultiplier = 1;
        }
    }

    [Serializable]
    public class EnemyAgressionPattern
    {
        [SerializeField] private EnemyMovementPattern _movement;
        [SerializeField] private float _speedMultipier = 1;
        [SerializeField] private int _meleeCombo;

        public EnemyMovementPattern Movement => _movement;
        public float SpeedMultipier => _speedMultipier;
        public int MeleeCombo => _meleeCombo;
    }
}