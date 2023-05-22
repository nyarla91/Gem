using System;
using Extentions;
using UnityEngine;

namespace Gameplay.Character
{
    public class Sight : Transformable
    {
        [SerializeField] [Tooltip("Degrees / second")] private float _rotationSpeed = 720;
        
        public void RotateTowardsDirection(Vector3 direction, bool instantly = false)
        {
            if (direction.Equals(Vector3.zero))
                return;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            float maxDegrees = instantly ? Single.MaxValue : (_rotationSpeed * Time.fixedDeltaTime);
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, targetRotation, maxDegrees);
        }
    }
}