using System;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public class LocalizedString
    {
        [SerializeField] [TextArea(3, 10)] private string _english;
        [SerializeField] [TextArea(3, 10)] private string _russian;

        public string Russian => _russian;
        public string English => _english;

        public string GetTranslation(int language) => language switch
        {
            0 => English,
            1 => Russian,
            _ => ""
        };
    }
}