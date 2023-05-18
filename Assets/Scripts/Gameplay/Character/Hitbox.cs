using Extentions;
using UnityEngine;

namespace Gameplay.Character
{
    public class Hitbox : MonoBehaviour
    {
        private VitalsPool _vitalsPool;
        private Movable _movable;
        public VitalsPool VitalsPool => _vitalsPool ??= GetComponent<VitalsPool>();

        public Movable Movable => _movable ??= GetComponent<Movable>();
        
        public void TakeHit(Hit hit)
        {
            VitalsPool?.TakeDamage(hit.Damage);
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