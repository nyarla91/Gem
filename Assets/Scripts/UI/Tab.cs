using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class Tab : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MenuWindow _window;
        [SerializeField] private Tab _leftTab;
        [SerializeField] private Tab _rightTab;
        public Tab LeftTab => _leftTab;
        public Tab RightTab => _rightTab;
        public bool Openned { get; private set; }

        public UnityEvent OnOpen;
        
        public void Open()
        {
            _window.Open();
            _canvasGroup.alpha = 1;
            Openned = true;
            OnOpen?.Invoke();
        }
        
        public void Close()
        {
            _window.Close();
            _canvasGroup.alpha = 0.5f;
            Openned = false;
        }
    }
}