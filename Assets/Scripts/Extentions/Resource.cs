using System;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class Resource
    {
        [SerializeField] private float _maxValue;

        private float _value;

        public float Value
        {
            get => _value;
            set
            {
                if (value > _maxValue)
                    GainedExcees?.Invoke(value - _maxValue);
                    
                value = Mathf.Clamp(value, 0, MaxValue);
                if (value.Equals(_value))
                    return;
                
                if (_value > 0 && value == 0)
                    Over?.Invoke();

                _value = value;
                Changed?.Invoke(_value, MaxValue);
            }
        }

        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public bool IsFull => Value.Equals(MaxValue);
        public bool IsNotFull => ! IsFull;

        public float Percent
        {
            get => Value / MaxValue;
            set => Value = MaxValue * value;
        }

        private ResourceWrap _wrap;
        public ResourceWrap Wrap => _wrap ??= new ResourceWrap(this);

        public delegate void OnChangeHandler(float current, float max);
        public event OnChangeHandler Changed;
        public event Action Over;
        public event Action<float> GainedExcees;

        public bool TrySpend(float value)
        {
            if (Value < value)
                return false;
            Value -= value;
            return true;
        }
    }

    public class ResourceWrap
    {
        private readonly Resource _resource;

        public float Value => _resource.Value;
        public float MaxValue => _resource.MaxValue;

        public bool IsFull => _resource.IsFull;
        public bool IsNotFull => _resource.IsNotFull;
        public float Percent => _resource.Percent;

        public event Resource.OnChangeHandler Changed;
        public event Action Over;
        public event Action<float> GainedExcees;

        public ResourceWrap(Resource resource)
        {
            _resource = resource;
            _resource.Changed += (current, max) => Changed?.Invoke(current, max);
            _resource.Over += () => Over?.Invoke();
            _resource.GainedExcees += excess => GainedExcees?.Invoke(excess);
        }
    }
}