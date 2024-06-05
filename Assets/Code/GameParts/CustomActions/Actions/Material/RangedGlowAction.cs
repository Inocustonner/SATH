using System;
using System.Collections;
using Code.Data.Interfaces;
using Code.Data.Values.RangeFloat;
using Code.Materials;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions.Material
{
    public class RangedGlowAction : CustomAction,IGameInitListener, IGameExitListener
    {
        [SerializeField,MinMaxRangeFloat(0, 3)] private RangedFloat _flashingDuration;
        [SerializeField, MinMaxRangeFloat(0, 100)] private RangedFloat _maxGlowValue;
        [SerializeField] private GlowMaterialController _material;
        
        private bool _isActive;
        private Coroutine _coroutine;
        
        public void GameInit()
        {
            _material.SetFloatValue(0);   
        }
        
        public void GameExit()
        {
            _material.SetFloatValue(0);
        }

        public override void StartAction()
        {
            _isActive = true;
            TryStopCoroutine();
            _coroutine = StartCoroutine(StartFlashing());
            InvokeStartActionEvent();
        }

        public override void StopAction()
        {
            _isActive = false;
            TryStopCoroutine();
            if (gameObject.activeInHierarchy)
            {
                _coroutine = StartCoroutine(StopFlashing(onEnd: InvokeEndActionEvent));
            }
            else
            {
                _material.SetFloatValue(0);
            }
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        private IEnumerator StartFlashing()
        {

            while (_isActive)
            {
                float halfDuration = _flashingDuration.GetRandomValue() / 2;
                float glowValue = _maxGlowValue.GetRandomValue();
                float elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(Mathf.Lerp(0, glowValue, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(100);
                elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(Mathf.Lerp(glowValue, 0, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(0);
            }
        }

        private IEnumerator StopFlashing(Action onEnd)
        {
            float halfDuration = _flashingDuration.GetRandomValue() / 2;
            float elapsedTime = 0.0f;
            float currentVaLue = _material.GetFloatValue();
            
            while (elapsedTime < halfDuration && _material.GetFloatValue() > 0)
            {
                _material.SetFloatValue(Mathf.Lerp(currentVaLue, 0, elapsedTime / halfDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _material.SetFloatValue(0);
            onEnd?.Invoke();
        }
    }
}