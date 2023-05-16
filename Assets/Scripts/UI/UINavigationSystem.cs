using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class UINavigationSystem : Transformable
    {
        [SerializeField] private AudioSource _navigationSoundSource;
        [SerializeField] private SelectionBox _selectionBox;

        private GameObject _currentlySelectetElement;
        private readonly List<Menu> _openedMenus = new List<Menu>();

        [Inject] private DeviceWatcher DeviceWatcher { get; set; }
        private EventSystem EventSystem => EventSystem.current;
        public MenuWindow LastOpenedInteractableWindow { get; set; }
        public bool PointerNavigation { get; private set; }
        public Menu FocusedMenu => _openedMenus.Last();

        public GameObject CurrentlySelectedElement
        {
            set
            {
                if (_currentlySelectetElement == value)
                    return;

                RectTransform rectTransform = value == null ? null : value.GetComponent<RectTransform>();
                
                MoveSelectionBox(rectTransform);
                _currentlySelectetElement = value;
            }
        }

        public void AddMenuOpen(Menu menu)
        {
            _openedMenus.Add(menu);
        }

        public void OnMenuClosed(Menu menu)
        {
            _openedMenus.TryRemove(menu);
            for (var i = _openedMenus.Count - 1; i >= 0; i--)
            {
                GameObject element = _openedMenus[i]?.CurrentWindow?.FirstSelected;
                if (element != null)
                    EventSystem.SetSelectedGameObject(element);
            }
        }

        private void Awake()
        {
            DeviceWatcher.OnUISchemeChanged += UpdateSelection;
            DeviceWatcher.OnUISchemeSet += MouseDeselect;
        }

        private void MouseDeselect(UIScheme scheme)
        {
            if (scheme != UIScheme.Pointer)
                return;
            
            PointerNavigation = true;
            StartCoroutine(Deselect());
        }

        private IEnumerator Deselect()
        {
            yield return null;
            EventSystem.SetSelectedGameObject(null);
        }

        private void MoveSelectionBox(RectTransform targetTransform)
        {
            if (DeviceWatcher.CurrentUIScheme == UIScheme.Pointer || targetTransform == null)
            {
                _selectionBox.Hide(RectTransform);
                return;
            }
            _selectionBox.MoveTo(targetTransform);
            _navigationSoundSource.pitch = Random.Range(0.9f, 1.1f);
            _navigationSoundSource.Play();
        }

        private void UpdateSelection(UIScheme scheme)
        {
            if (scheme == UIScheme.Pointer)
                return;
            
            PointerNavigation = false;
            if (LastOpenedInteractableWindow != null && LastOpenedInteractableWindow.IsOpened)
                EventSystem.SetSelectedGameObject(LastOpenedInteractableWindow.FirstSelected);
        }
        
        private void Update()
        {
            CurrentlySelectedElement = EventSystem.currentSelectedGameObject;
        }

        private void OnDestroy()
        {
            DeviceWatcher.OnUISchemeChanged -= UpdateSelection;
            DeviceWatcher.OnUISchemeSet -= MouseDeselect;
        }
    }
}