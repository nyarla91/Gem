using System;
using System.Linq;
using Extentions;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace UI
{
    public sealed class Menu : MonoBehaviour
    {
        [SerializeField] private bool _independent = true;
        [SerializeField] private bool _openedAtStart;
        [SerializeField] private bool _pausesTheGame;
        [SerializeField] private MenuWindow _firstMenuWindow;

        private MenuWindow[] _windows;
        
        public bool IsOpened { get; private set; }
        public MenuWindow CurrentWindow { get; private set; }
        
        public MenuWindow FirstMenuWindow => _firstMenuWindow;
        
        [Inject] private IPauseChanger Pause { get; set; }
        [Inject] private UINavigationSystem NavigationSystem { get; set; }

        public event Action OnOpen;
        public event Action OnClose;

        public UnityEvent UnityOnOpen;
        public UnityEvent UnityOnClose;

        public void SwitchToWindow(MenuWindow menuWindow)
        {
            if ( ! _windows.Contains(menuWindow) || ! IsOpened)
                return;
            
            foreach (MenuWindow searchedWindow in _windows)
            {
                if (searchedWindow == menuWindow)
                {
                    searchedWindow.Open();
                    CurrentWindow = searchedWindow;
                    continue;
                }
                searchedWindow.Close();
            }
        }
        
        public void Open()
        {
            IsOpened = true;
            if (_independent)
                NavigationSystem.AddMenuOpen(this);
            if (_pausesTheGame)
                Pause.AddPauseSource(this);
            if (_firstMenuWindow)
                SwitchToWindow(_firstMenuWindow);
            OnOpen?.Invoke();
            UnityOnOpen?.Invoke();
        }

        public void Close()
        {
            IsOpened = false;
            if (_independent)
                NavigationSystem.OnMenuClosed(this);
            if (_pausesTheGame)
                Pause.RemovePauseSource(this);
            foreach (MenuWindow searchedWindow in _windows)
            {
                searchedWindow.Close();
            }
            OnClose?.Invoke();
            UnityOnClose?.Invoke();
        }

        private void Awake()
        {
            _windows = GetComponentsInChildren<MenuWindow>();
        }

        private void Start()
        {
            Close();
            if (_openedAtStart)
                Open();
        }
    }
}