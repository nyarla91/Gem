using Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movable : LazyGetComponent<Rigidbody>
    {
        public Vector3 Velocity { get; set; }

        [Inject] private IPauseInfo Pause { get; set; }
        
        private void FixedUpdate()
        {
            Lazy.velocity = Pause.IsUnpaused ? Velocity : Vector3.zero;
        }
    }
}