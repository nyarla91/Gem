using System;
using Extentions;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public struct SettingsSection
    {
        [SerializeField] private SerializedDictionary<string, int> _settings;

        public void SetSetting(string setting, int value)
        {
            ValidateSetting(setting);
            SerializedKeyValuePair<string,int> pair = _settings.Pairs.Find(pair => pair.Key.Equals(setting));
            pair.Value = value;
        }

        public int GetSettingValue(string setting)
        {
            ValidateSetting(setting);
            return _settings.Dictionary[setting];
        }

        public bool IsSettingToggled(string setting)
        {
            ValidateSetting(setting);
            return _settings.Dictionary[setting] == 1;
        }

        private void ValidateSetting(string setting)
        {
            if (!_settings.Dictionary.ContainsKey(setting))
                throw new ArgumentOutOfRangeException($"There is no {setting} setting");
        }
    }
}