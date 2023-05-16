using System;
using System.IO;
using Localization;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AnimationCurve _soundCurve;
        [SerializeField] private string _savedFileName;
        [SerializeField] private SettingsConfig _config;

        public SettingsConfig Config => _config;

        private string SaveFilePath => Application.dataPath + "/" + _savedFileName + ".json";

        public event Action ConfigChanged;
        
        private void Awake()
        {
            if (TryLoad(out SettingsConfig loadedConfig))
                _config = loadedConfig;
            Apply(Config);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
                Screen.fullScreenMode = FullScreenMode.Windowed;
            if (UnityEngine.Input.GetKeyDown(KeyCode.H))
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }

        private bool TryLoad(out SettingsConfig loadedConfig)
        {
            loadedConfig = new SettingsConfig();
            if (!File.Exists(SaveFilePath))
                return false;
            string json = File.ReadAllText(SaveFilePath);
            loadedConfig = JsonUtility.FromJson<SettingsConfig>(json);
            return true;
        }

        public void SaveAndApply()
        {
            string json = JsonUtility.ToJson(Config);
            File.WriteAllText(SaveFilePath, json);
            Apply(Config);
            ConfigChanged?.Invoke();
        } 

        private void Apply(SettingsConfig config)
        {
            FullScreenMode fullScreenMode = config.Video.IsSettingToggled("fullscreen") ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.fullScreenMode = fullScreenMode;
            int resolution = Config.Video.GetSettingValue("resolution");
            int screenWidth = resolution switch
            {
                0 => 960,
                1 => 1280,
                2 => 1366,
                3 => 1920,
                4 => 2560,
                5 => 3840,
                _ => throw new ArgumentOutOfRangeException()
            };
            int screenHeight = resolution switch
            {
                0 => 540,
                1 => 720,
                2 => 768,
                3 => 1080,
                4 => 1440,
                5 => 2160,
                _ => throw new ArgumentOutOfRangeException()
            };
            Screen.SetResolution(screenWidth, screenHeight, fullScreenMode);

            _mixer.SetFloat("MusicVolume", _soundCurve.Evaluate(config.Audio.GetSettingValue("music volume")));
            _mixer.SetFloat("SFXVolume", _soundCurve.Evaluate(config.Audio.GetSettingValue("sfx volume")));

            LocalizedTextMesh.Language = config.Game.GetSettingValue("language");
        }
    }
}