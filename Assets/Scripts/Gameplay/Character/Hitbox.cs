using Extentions;
using UnityEngine;

namespace Gameplay.Character
{
    public class Hitbox : MonoBehaviour
    {
        private VitalsPool _vitalsPool;
        private Movable _movable;
        private Disorientation _disorientation;

        private VitalsPool VitalsPool => _vitalsPool ??= GetComponent<VitalsPool>();
        private Movable Movable => _movable ??= GetComponent<Movable>();

        private Disorientation Disorientation => _disorientation ??= GetComponent<Disorientation>();
        
        public void TakeHit(Hit hit)
        {
            VitalsPool?.TakeDamage(hit.Damage);
            Disorientation?.Stagger();
            if (Movable != null) 
                Movable.ForceVelocity = hit.PushForce * -1 * hit.DirectionFrom;
        }
    }

    public class Hit
    {
        public int Damage { get; }
        public Vector3 DirectionFrom { get; }
        public float PushForce { get; }

        public Hit(int damage, Vector3 directionFrom, float pushForce = 0)
        {
            Damage = damage;
            DirectionFrom = directionFrom.WithY(0).normalized;
            PushForce = pushForce;
        }
    }
}