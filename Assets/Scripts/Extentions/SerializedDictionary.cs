using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue>
    {
        [SerializeField] [Tooltip("If unchecked, Dictionary will only be generated once")] private bool _dynamic;
        [SerializeField] private List<SerializedKeyValuePair<TKey, TValue>> _pairs;

        private Dictionary<TKey, TValue> _dictionary;
        
        public List<SerializedKeyValuePair<TKey, TValue>> Pairs
        {
            get
            {
                if (!_dynamic)
                    throw new AccessViolationException("You can't get Pairs of non-dynamic SerializedDictionary");
                return _pairs;
            }
        }

        public Dictionary<TKey, TValue> Dictionary => _dynamic ? GenerateDictionary() : (_dictionary = GenerateDictionary());

        private Dictionary<TKey, TValue> GenerateDictionary()
        {
            return _pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    [Serializable]
    public class SerializedKeyValuePair<TKey, TValue>
    {
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public TKey Key
        {
            get => _key;
            set => _key = value;
        }

        public TValue Value
        {
            get => _value;
            set => _value = value;
        }
    }
}