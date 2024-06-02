using System;
using Code.Data.Enums;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Replicas
{
    public abstract class GamePart : MonoBehaviour, IEntity
    {
        public abstract GamePartName GamePartName { get; }
        public event Action OnUpdatePartData;
        public event Action<GamePart> OnTryRestart;
        
        
        public abstract void Reset();

        protected void InvokeUpdateDataEvent()
        {
           OnUpdatePartData?.Invoke();
        }

        protected void InvokeTryRestartEvent()
        {
            OnTryRestart?.Invoke(this);
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