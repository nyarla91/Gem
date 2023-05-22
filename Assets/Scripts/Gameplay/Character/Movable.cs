using Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movable : LazyGetComponent<Rigidbody>
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration = 10;

        private const float ForceFadeMultiplier = 0.9f;
        private const float ForceVelocityThreshold = 0.5f;

        private Vector3 _voluntaryVelocity;
        public Vector3 VoluntaryVelocity
        {
            get => _voluntaryVelocity;
            set => DesiredVelocity = _voluntaryVelocity = value;
        }

        private Vector3 DesiredVelocity { get; set; }
        public Vector3 ForceVelocity { get; set; }
        public float SpeedMultiplier { get; set; } = 1;

        [Inject] private IPauseInfo Pause { get; set; }

        public void Walk(Vector3 direction) => DesiredVelocity = direction * _maxSpeed * SpeedMultiplier;

        private void FixedUpdate()
        {
            VoluntaryVelocity = Vector3.MoveTowards(VoluntaryVelocity, DesiredVelocity,
                _maxSpeed * SpeedMultiplier * _acceleration * Time.fixedDeltaTime);
            
            Lazy.velocity = Pause.IsUnpaused
                ? (ForceVelocity.magnitude >= ForceVelocityThreshold ? ForceVelocity : VoluntaryVelocity)
                : Vector3.zero;
            ForceVelocity *= ForceFadeMultiplier;
        }
    }
}