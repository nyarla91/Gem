using UnityEngine;
using UnityEngine.UI;

namespace Settings.UI
{
    public class FlagSettingMenuItem : SettingMenuItem
    {
        [SerializeField] private Toggle _toggle;

        public void SwitchToggle()
        {
            _toggle.isOn = !_toggle.isOn;
        }
        
        public void OnValueChanged(bool value) => ApplySetting(value ? 1 : 0);
        protected override void SetStartingValue(int value)
        {
            _toggle.isOn = value == 1;
        }
    }
}