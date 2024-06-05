using System;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions
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

        protected void InvokeStartActionEvent()
        {
            InProgress = true;
            OnStart?.Invoke();
        }

        protected void InvokeEndActionEvent()
        {
            InProgress = false;
            OnEnd?.Invoke();
        }
    }
}