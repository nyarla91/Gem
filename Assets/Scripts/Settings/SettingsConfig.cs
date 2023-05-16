using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public struct SettingsConfig
    {
        [SerializeField] private SettingsSection _video;
        [SerializeField] private SettingsSection _audio;
        [SerializeField] private SettingsSection _game;
        
        public SettingsSection Video => _video;
        public SettingsSection Audio => _audio;
        public SettingsSection Game => _game;

        public SettingsSection GetSection(SettingsSectionLabel label)
        {
            return label switch
            {
                SettingsSectionLabel.Video => Video,
                SettingsSectionLabel.Audio => Audio,
                SettingsSectionLabel.Game => Game,
                _ => throw new ArgumentOutOfRangeException(nameof(label), label, null)
            };
        }
    }

    public enum SettingsSectionLabel
    {
        Video,
        Audio,
        Game,
    }
}