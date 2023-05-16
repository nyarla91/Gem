using System;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private bool _setAlpha = true;
        [SerializeField] private GameObject _firstSelected;
        [SerializeField] private bool _canEscape = true;
        [SerializeField] private MenuWindow _previousMenuWindow;

        private bool _isOpened = true;
        private UINavigationSystem _navigationSystem;
        private CanvasGroup _canvasGroup;

        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

        private EventSystem _eventSystem;

        public GameObject FirstSelected => _firstSelected;
        public bool IsOpened => _isOpened;
        private UINavigationSystem NavigationSystem => _navigationSystem ??= GameObject.FindObjectOfType<UINavigationSystem>();
        private EventSystem EventSystem => _eventSystem ??= EventSystem.current;

        private Menu Menu { get; set; }

        public event Action OnOpen;
        public event Action OnClose;

        public UnityEvent UnityOnOpen;
        public UnityEvent UnityOnClose;

        public virtual void Open()
        {
            if (_isOpened)
                return;
            _isOpened = true;
            OnOpen?.Invoke();
            UnityOnOpen?.Invoke();
            
            if (_firstSelected != null && ! NavigationSystem.PointerNavigation)
                EventSystem.SetSelectedGameObject(_firstSelected);
            
            if (_setAlpha)
                CanvasGroup.alpha = 1;
            CanvasGroup.interactable = CanvasGroup.blocksRaycasts = true;
            
            if (_canEscape)
                MenuControls.Actions.Always.Cancel.performed += SwitchToPreviousWindow;
            
            if (_firstSelected != null)
                NavigationSystem.LastOpenedInteractableWindow = this;
        }

        public void SwitchToThis()
        {
            print(Menu);
            Menu.SwitchToWindow(this);
        }

        public virtual void Close()
        {
            if ( ! _isOpened)
                return;
            OnClose?.Invoke();
            UnityOnClose?.Invoke();

            if (_setAlpha)
                CanvasGroup.alpha = 0;
            CanvasGroup.interactable = CanvasGroup.blocksRaycasts = false;
            
            _isOpened = false;
            
            if (_canEscape)
                MenuControls.Actions.Always.Cancel.performed -= SwitchToPreviousWindow;
        }

        public void SwitchToPreviousWindow() => SwitchToPreviousWindow(new InputAction.CallbackContext());

        public void SwitchToPreviousWindow(InputAction.CallbackContext _)
        {
            if (NavigationSystem.FocusedMenu != Menu)
                return;
            if (_previousMenuWindow != null)
                Menu.SwitchToWindow(_previousMenuWindow);
            else
                Menu?.Close();
        }

        private void Awake()
        {
            Menu = GetComponentInParent<Menu>();
        }

        private void OnDestroy()
        {
            if (_canEscape)
                MenuControls.Actions.Always.Cancel.performed -= SwitchToPreviousWindow;
        }
    }
}