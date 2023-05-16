using System;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class Range
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;
        public float Random => UnityEngine.Random.Range(Min, Max);

        private void OnValidate()
        {
            if (Max < Min)
                _max = _min;
        }
    }
}