using Input;
using UI.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Settings.UI
{
    public class SliderSettingMenuItem : SettingMenuItem
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private int _pointsPerClick = 1;

        private UIElement _uiElement;

        public void OnValueChanged(float value) => ApplySetting(Mathf.RoundToInt(value));
        
        protected override void SetStartingValue(int value) => _slider.value = value;

        private void Awake()
        {
            _uiElement = GetComponent<UIElement>();
            MenuControls.Actions.Always.MoveLeft.performed += TryMoveLeft;
            MenuControls.Actions.Always.MoveRight.performed += TryMoveRight;
        }

        private void TryMoveRight(InputAction.CallbackContext callbackContext)
        {
            if (!_uiElement.IsSelected)
                return;
            MoveScrollbar(_pointsPerClick);
        }

        private void TryMoveLeft(InputAction.CallbackContext callbackContext)
        {
            if (!_uiElement.IsSelected)
                return;
            MoveScrollbar(-_pointsPerClick);
        }

        private void MoveScrollbar(int delta) => _slider.value += delta;

        private void OnDestroy()
        {
            MenuControls.Actions.Always.MoveLeft.performed -= TryMoveLeft;
            MenuControls.Actions.Always.MoveRight.performed -= TryMoveRight;
        }
    }
}