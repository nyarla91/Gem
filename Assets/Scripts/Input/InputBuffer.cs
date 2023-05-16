using System;
using Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputBuffer
    {
        private readonly Timer _timer;
        private float _timeWindow;
        
        public float TimeWindow
        {
            get => _timeWindow;
            set
            {
                _timeWindow = value;
                _timer.Length = value;
            }
        }

        private bool PerformAllowed => PerformCondition == null || PerformCondition.Invoke();

        public event Func<bool> PerformCondition;

        public event Action Performed;

        public InputBuffer(MonoBehaviour container, float timeWindow)
        {
            _timeWindow = timeWindow;
            _timer = new Timer(container, timeWindow);
            _timer.Ticked += CheckAvialability;
        }

        public void SendInput(InputAction.CallbackContext context) => SendInput();
        
        public void SendInput()
        {
            _timer.Stop();
            if (PerformAllowed)
            {
                Performed?.Invoke();
                return;
            }
            _timer.Restart();
        }

        public void InterruptBuffering() => _timer.Stop();

        private void CheckAvialability(float irrelevant)
        {
            if ( ! PerformAllowed)
                return;
            
            Performed?.Invoke();
            _timer.Stop();
        }
    }
}