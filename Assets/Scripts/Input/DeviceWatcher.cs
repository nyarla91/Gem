using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.SceneManagement;

namespace Input
{
    public class DeviceWatcher : MonoBehaviour
    {
        private DeviceUpdateActions _deviceUpdateActions;
        private MenuActions _menuActions;
        private InputScheme _currentInputScheme = InputScheme.None;
        private UIScheme _currentUIScheme = UIScheme.None;
        private GamepadModel _currentGamepadModel = GamepadModel.None;
        
        public InputScheme CurrentInputScheme
        {
            get => _currentInputScheme;
            private set
            {
                if (value == _currentInputScheme)
                    return;
                _currentInputScheme = value;
                OnInputSchemeChanged?.Invoke(value);
            }
        }
        
        public GamepadModel CurrentGamepadModel
        {
            get => _currentGamepadModel;
            private set
            {
                if (value == _currentGamepadModel)
                    return;
                _currentGamepadModel = value;
                OnGamepadModelChanged?.Invoke(value);
            }
        }

        public UIScheme CurrentUIScheme
        {
            get => _currentUIScheme;
            set
            {
                OnUISchemeSet?.Invoke(value);
                if (value == _currentUIScheme)
                    return;
                _currentUIScheme = value;
                OnUISchemeChanged?.Invoke(value);
            }
        }
        
        public event Action<InputScheme> OnInputSchemeChanged;
        public event Action<GamepadModel> OnGamepadModelChanged;
        public event Action<UIScheme> OnUISchemeChanged;
        public event Action<UIScheme> OnUISchemeSet;

        private void Awake()
        {
            _deviceUpdateActions = new DeviceUpdateActions();
            _deviceUpdateActions.General.Enable();
            _deviceUpdateActions.General.Keyboard.started += SwitchToKeyboard;
            _deviceUpdateActions.General.Mouse.started += SwitchToMouse;
            _deviceUpdateActions.General.Gamepad.started += SwitchToGamepad;
            _deviceUpdateActions.General.DualShock.started += SwitchToDualshock;
            _deviceUpdateActions.General.Navigation.started += SwitchToNavigation;
            SceneManager.sceneLoaded += ResetSchemes;
            DontDestroyOnLoad(gameObject);
        }

        private void ResetSchemes(Scene arg0, LoadSceneMode arg1)
        {
            _currentInputScheme = InputScheme.None;
            _currentUIScheme = UIScheme.None;
            _currentGamepadModel = GamepadModel.Xbox;
        }

        private void SwitchToNavigation(InputAction.CallbackContext context)
        {
            CurrentUIScheme = UIScheme.Navigation;
        }

        private void SwitchToKeyboard(InputAction.CallbackContext context)
        {
            CurrentInputScheme = InputScheme.KeyboardMouse;
        }

        private void SwitchToMouse(InputAction.CallbackContext context)
        {
            CurrentInputScheme = InputScheme.KeyboardMouse;
            CurrentUIScheme = UIScheme.Pointer;
        }

        private void SwitchToGamepad(InputAction.CallbackContext context)
        {
            CurrentInputScheme = InputScheme.Gamepad;
            CurrentUIScheme = UIScheme.Navigation;

            Gamepad gamepad = Gamepad.current;
            CurrentGamepadModel = gamepad switch
            {
                DualShockGamepad => GamepadModel.PlayStation,
                SwitchProControllerHID => GamepadModel.Switch,
                _ => GamepadModel.Xbox
            };
        }

        private void SwitchToDualshock(InputAction.CallbackContext context)
        {
            CurrentInputScheme = InputScheme.Gamepad;
            CurrentGamepadModel = GamepadModel.PlayStation;
            CurrentUIScheme = UIScheme.Navigation;
        }
    }

    public enum GamepadModel
    {
        None,
        PlayStation,
        Xbox,
        Switch,
    }

    public enum InputScheme
    {
        None,
        KeyboardMouse,
        Gamepad,
    }

    public enum UIScheme
    {
        Navigation,
        Pointer,
        None,
    }
}
