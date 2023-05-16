using UnityEngine;
using Zenject;

namespace Settings.UI
{
    public abstract class SettingMenuItem : MonoBehaviour
    {
        [SerializeField] private SettingsSectionLabel _section;
        [SerializeField] private string _settingLabel;

        [Inject] private Settings Settings { get; set; }

        private SettingsSection TargetSection => Settings.Config.GetSection(_section);

        protected void ApplySetting(int value)
        {
            if (Settings == null)
                return;
            TargetSection.SetSetting(_settingLabel, value);
            Settings.SaveAndApply();
        }

        protected abstract void SetStartingValue(int value);

        private void Start()
        {
            SetStartingValue(TargetSection.GetSettingValue(_settingLabel));
        }
    }
}