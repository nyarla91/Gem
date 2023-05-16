using System.Threading.Tasks;
using Core;
using Extentions;
using Input;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static Extentions.Pause.Pause;

namespace Gameplay.UI
{
    public class InGameMenu : LazyGetComponent<Menu>
    {
        [Inject] private SceneLoader SceneLoader { get; set; }
        [Inject] private IPauseInfo Pause { get; set; }

        public void Quit() => Application.Quit();
        public void Surrender() => SceneLoader.LoadMainMenu();

        private void Awake()
        {
            Lazy.OnOpen += UnsubscribeMenuOpen;
            Lazy.OnClose += SubscribeMenuOpen;
        }
        
        private async void SubscribeMenuOpen()
        {
            await Task.Delay(1);
            MenuControls.Actions.Always.OpenMenu.started += OpenMenuIfClosed;
        }

        private void UnsubscribeMenuOpen() => MenuControls.Actions.Always.OpenMenu.started -= OpenMenuIfClosed;

        private void OpenMenuIfClosed(InputAction.CallbackContext _)
        {
            if (Pause.IsPaused)
                return;
            Lazy.Open();
        }

        private void OnDestroy()
        {
            if ( ! Lazy.IsOpened)
                UnsubscribeMenuOpen();
        }
    }
}