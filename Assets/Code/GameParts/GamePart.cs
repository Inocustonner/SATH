using System;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.GameParts
{
    [RequireComponent(typeof(PartEventDispatcher))]
    public abstract class GamePart : MonoBehaviour, IEntity
    {
        public abstract GamePartName GamePartName { get; }
        public event Action OnUpdatePartData;
        public event Action<GamePart> OnTryRestart;
        public event Action OnRestart;

        public virtual void Restart()
        {
            InvokeRestartEvent();
        }
        
        protected void InvokeUpdateDataEvent()
        {
           OnUpdatePartData?.Invoke();
        }

        protected void InvokeTryRestartEvent()
        {
            OnTryRestart?.Invoke(this);
        }

        protected void InvokeRestartEvent()
        {
            OnRestart?.Invoke();
        } 
        
        #region Editor

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            gameObject.name = $"{GamePartName}";
        }
#endif
        #endregion
    }
}