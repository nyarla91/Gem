using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.UIElements
{
    [RequireComponent(typeof(Selectable))]
    [RequireComponent(typeof(EventTrigger))]
    public sealed class UIElement : MonoBehaviour, IRemovableHandler
    {
        private Selectable _selectable;

        public Selectable Selectable => _selectable ??= GetComponent<Selectable>();
        private EventSystem EventSystem => EventSystem.current;
        [Inject] private UINavigationSystem UINavigationSystem { get; set; }
        public bool IsSelected => EventSystem.currentSelectedGameObject == gameObject;

        public UnityEvent OnPress;

        public void Press(BaseEventData _) => Press();
        
        public void Press()
        {
            OnPress?.Invoke();
        }

        public void OnRemoved()
        {
            if (!IsSelected)
                return;
            
            Selectable closest = GetClosestSelectable(true);
            GameObject closestElement = closest == null ? null : closest.gameObject; 
            EventSystem.SetSelectedGameObject(closestElement);
            UINavigationSystem.CurrentlySelectedElement = closestElement;
        }

        private Selectable GetClosestSelectable(bool excludeSiblings)
        {
            Selectable[] allSelectables = Selectable.allSelectablesArray;
            allSelectables = allSelectables.Where(selectable => selectable != Selectable).ToArray();
            if (excludeSiblings)
                allSelectables = allSelectables.Where(selectable => selectable.transform.parent != transform.parent).ToArray();
                    
            if (allSelectables.Length == 0)
                return null;
            
            allSelectables = allSelectables.OrderBy(selectable => Vector3.Distance(transform.position, selectable.transform.position)).ToArray();
            return allSelectables[0];
        }
    }
}