using System;
using Extentions;
using Gameplay.Character.Player;
using UnityEngine;
using Zenject;
using Range = Extentions.Range;

namespace Gameplay.Character.Enemy
{
    [RequireComponent(typeof(Sight))]
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(Movable))]
    public class EnemyMovement : Transformable
    {
        [SerializeField] private EnemyMovementPattern _defaultPattern;
        
        private Movable _movable;
        private Sight _sight;
        private StateMachine _stateMachine;

        private Movable Movable => _movable ??= GetComponent<Movable>();

        public Sight Sight => _sight ??= GetComponent<Sight>();
        private StateMachine StateMachine => _stateMachine ??= GetComponent<StateMachine>();

        public EnemyMovementPattern CurrentPattern { get; set; }
        private float DistanceToPlayer => Vector3.Distance(Transform.position, Player.Position);
        public Vector3 DirectionToPlayer => Transform.DirectionTo(Player.Position);
        public bool IsWithinDesiredRange => CurrentPattern.GetApproachDirection(DistanceToPlayer) == 0;
        public int ApproachDirection => CurrentPattern.GetApproachDirection(DistanceToPlayer);

        [Inject] private IPlayerTransformInfo Player { get; set; }


        public void RevertDefaultPattern() => CurrentPattern = _defaultPattern;


        private void Awake()
        {
            RevertDefaultPattern();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (StateMachine.IsCurrentStateNoneOf(StateMachine.Regular))
                return;
            Movable.Walk(DirectionToPlayer * ApproachDirection);
            Sight.RotateTowardsDirection(DirectionToPlayer);
        }
    }

    [Serializable]
    public class EnemyMovementPattern
    {
        [SerializeField] private Range _desiredDistance;

        public int GetApproachDirection(float distance) => _desiredDistance.UnfitSign(distance);
    }
}