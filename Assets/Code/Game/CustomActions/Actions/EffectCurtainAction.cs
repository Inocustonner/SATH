using System;
using System.Collections;
using Code.Data.Enums;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class EffectCurtainAction: CustomAction, IEntity
    {
        [SerializeField] private EffectCurtainType _curtainType;
        [SerializeField] private float _duration = 1;

        public event Action<EffectCurtainType, float> OnTryShowEffectCurtain; 
        private Coroutine _coroutine;
        
        public override void StartAction()
        {
            TryStopCoroutine();
            InvokeStartActionEvent();            
            OnTryShowEffectCurtain?.Invoke(_curtainType,_duration);
        }

        public override void StopAction()
        {
            InvokeEndActionEvent();            
        }

        private void OnDisable()
        {
            TryStopCoroutine();
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
        
        private IEnumerator StopWithDelay()
        {
            yield return new WaitForSeconds(_duration);
            StopAction();
        }
    }
}