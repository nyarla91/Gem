using System;
using Extentions;
using Input;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class DeviceBasedInputPrompts : LazyGetComponent<TMP_Text>
    {
        [SerializeField] private TMP_SpriteAsset _keyboardMouse;
        [SerializeField] private TMP_SpriteAsset _xbox;
        [SerializeField] private TMP_SpriteAsset _playStation;

        [Inject] private DeviceWatcher DeviceWatcher { get; set; }

        private void Awake()
        {
            DeviceWatcher.OnGamepadModelChanged += OnGamepadModelChanged;
            DeviceWatcher.OnInputSchemeChanged += OnInputSchemeChanged;
            UpdatePrompts();
        }

        private void OnGamepadModelChanged(GamepadModel _) => UpdatePrompts();
        private void OnInputSchemeChanged(InputScheme _) => UpdatePrompts();

        private void UpdatePrompts()
        {
            Lazy.spriteAsset = DeviceWatcher.CurrentInputScheme switch
            {
                InputScheme.KeyboardMouse => _keyboardMouse,
                InputScheme.None => _keyboardMouse,
                InputScheme.Gamepad => DeviceWatcher.CurrentGamepadModel switch
                {
                    GamepadModel.PlayStation => _playStation,
                    GamepadModel.Xbox => _xbox,
                    GamepadModel.Switch => _xbox,
                    GamepadModel.None => _xbox,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}