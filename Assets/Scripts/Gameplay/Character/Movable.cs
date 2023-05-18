using Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movable : LazyGetComponent<Rigidbody>
    {
        private const float ForceFadeMultiplier = 0.9f;
        private const float ForceVelocityThreshold = 0.5f;
        
        public Vector3 VoluntaryVelocity { get; set; }
        public Vector3 ForceVelocity { get; set; }

        [Inject] private IPauseInfo Pause { get; set; }

        private void FixedUpdate()
        {
            print($"{gameObject} {ForceVelocity}");
            Lazy.velocity = Pause.IsUnpaused
                ? (ForceVelocity.magnitude >= ForceVelocityThreshold ? ForceVelocity : VoluntaryVelocity)
                : Vector3.zero;
            ForceVelocity *= ForceFadeMultiplier;
        }
    }
}