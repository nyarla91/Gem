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

         /// <summary>
         /// Returns -1 if value is lesser than Min, 1 if value is greater than Max, 0 if value firs the range
         /// </summary>
        public int UnfitSign(float value) => value < Min ? -1 : (value > Max ? 1 : 0);

        private void OnValidate()
        {
            if (Max < Min)
                _max = _min;
        }
    }
}