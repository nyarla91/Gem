using System;
using Extentions;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI
{
    public abstract class MenuAdditionalActions<T> : LazyGetComponent<T> where T : MonoBehaviour
    {
        protected abstract Func<T, bool> TriggerCondition { get; }
        
        private bool CanTrigger => TriggerCondition.Invoke(Lazy);

        public UnityEvent OnAction1;
        public UnityEvent OnAction2;
        public UnityEvent OnAction3;
        public UnityEvent OnAction4;

        private void Awake()
        {
            
            Subscribe();
        }

        private void Subscribe()
        {
            MenuControls.Actions.Always.Action1.performed += TryAction1;
            MenuControls.Actions.Always.Action2.performed += TryAction2;
            MenuControls.Actions.Always.Action3.performed += TryAction3;
            MenuControls.Actions.Always.Action4.performed += TryAction4;
        }

        private void TryAction1(InputAction.CallbackContext _)
        {
            if (!CanTrigger)
                return;
            PerformAction(1);
        }
        private void TryAction2(InputAction.CallbackContext _)
        {
            if (!CanTrigger)
                return;
            PerformAction(2);
        }
        private void TryAction3(InputAction.CallbackContext _)
        {
            if (!CanTrigger)
                return;
            PerformAction(3);
        }
        private void TryAction4(InputAction.CallbackContext _)
        {
            if (!CanTrigger)
                return;
            PerformAction(4);
        }

        private void PerformAction(int index)
        {
            switch (index)
            {
                case 1: OnAction1?.Invoke(); break;
                case 2: OnAction2?.Invoke(); break;
                case 3: OnAction3?.Invoke(); break;
                case 4: OnAction4?.Invoke(); break;
                default: throw new ArgumentException();
            }
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            MenuControls.Actions.Always.Action1.performed -= TryAction1;
            MenuControls.Actions.Always.Action2.performed -= TryAction2;
            MenuControls.Actions.Always.Action3.performed -= TryAction3;
            MenuControls.Actions.Always.Action4.performed -= TryAction4;
        }
    }
}