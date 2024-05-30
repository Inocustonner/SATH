using System;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public abstract class CustomAction : MonoBehaviour
    {
        public bool InProgress { get; private set; }
        public event Action OnStart;
        public event Action OnEnd;

        public abstract void StartAction();
        public virtual void StopAction()
        {
        }

        protected void InvokeStartEvent()
        {
            InProgress = true;
            OnStart?.Invoke();
        }

        protected void InvokeEndEvent()
        {
            InProgress = false;
            OnEnd?.Invoke();
        }
    }
}