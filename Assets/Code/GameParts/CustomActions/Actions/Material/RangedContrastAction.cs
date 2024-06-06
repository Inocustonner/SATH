using System;
using System.Collections;
using Code.Data.Interfaces;
using Code.Data.Values.RangeFloat;
using Code.Materials;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions.Material
{
    public class RangedContrastAction : CustomAction,IGameInitListener, IGameExitListener
    {
        [SerializeField,MinMaxRangeFloat(0, 3)] private RangedFloat _flashingDuration;
        [SerializeField, MinMaxRangeFloat(0, 6)] private RangedFloat _maxContrastValue;
        [SerializeField] private ContrastMaterialController _material;
        
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
                float glowValue = _maxContrastValue.GetRandomValue();
                float minValue = _maxContrastValue.MinValue;
                float elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(Mathf.Lerp(minValue, glowValue, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(glowValue);
                elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(Mathf.Lerp(glowValue, minValue, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(minValue);
            }
        }

        private IEnumerator StopFlashing(Action onEnd)
        {
            float halfDuration = _flashingDuration.GetRandomValue() / 2;
            float elapsedTime = 0.0f;
            float currentVaLue = _material.GetFloatValue();
            float minValue = _maxContrastValue.MinValue;
            
            while (elapsedTime < halfDuration && _material.GetFloatValue() > minValue)
            {
                _material.SetFloatValue(Mathf.Lerp(currentVaLue, minValue, elapsedTime / halfDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _material.SetFloatValue(minValue);
            onEnd?.Invoke();
        }
    }
}