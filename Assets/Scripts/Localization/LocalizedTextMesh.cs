using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizedTextMesh : MonoBehaviour
    {
        public static int Language { get; set; } = 0;
        
        [SerializeField] private TMP_Text _mesh;
        [SerializeField] private LocalizedString _text;
        
        private Dictionary<string, string> _keys = new Dictionary<string, string>();
        
        public LocalizedString Text
        {
            get => _text;
            set
            {
                _text = value;
                ApplyString();
            }
        }

        public void AddKeyReplacement(string key, string value)
        {
            if (!_keys.ContainsKey(key))
                _keys.Add(key, value);
            ApplyString();
        }

        private void Start()
        {
            ApplyString();
        }

        private void ApplyString()
        {
            if (_mesh == null)
                return;
            string displayText = _text.GetTranslation(Language);
            foreach (KeyValuePair<string,string> pair in _keys)
            {
                displayText = displayText.Replace(pair.Key, pair.Value);
            }
            _mesh.text = displayText;
        }

        private void OnValidate()
        {
            if (_mesh == null)
                _mesh = GetComponent<TMP_Text>();
            else
                ApplyString();
        }
    }

}