using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ScreenShake : MonoBehaviour
    {
        [SerializeField] [Range(0, 1)] private float _fade;
        [SerializeField] [Range(0, 1)] private float _smoothing;
        private float _shake;
        
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.U))
                _shake += 1;
            transform.localRotation = Quaternion.LookRotation(Vector3.forward + (Vector3) Random.insideUnitCircle * _shake * _smoothing);
        }

        private void FixedUpdate()
        {
            _shake *= _fade;
        }
    }
}