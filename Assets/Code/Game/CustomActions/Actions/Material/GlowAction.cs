using System;
using System.Collections;
using Code.Data.Interfaces;
using Code.Materials;
using UnityEngine;

namespace Code.Game.CustomActions.Actions.Material
{
    public class GlowAction : CustomAction, IGameInitListener, IPartExitListener
    {
        [SerializeField] private float _flashingDuration = 4;
        [SerializeField, Range(0, 100)] private float _maxGlowValue = 50;
        [SerializeField] private GlowMaterialController _material;
        
        private bool _isActive;
        private Coroutine _coroutine;
        
        public void GameInit()
        {
            _material.SetFloatValue(0);   
        }
        
        public void PartExit()
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
            float halfDuration = _flashingDuration / 2;

            while (_isActive)
            {
                float elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(Mathf.Lerp(0, _maxGlowValue, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(100);
                elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(Mathf.Lerp(_maxGlowValue, 0, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(0);
            }
        }

        private IEnumerator StopFlashing(Action onEnd)
        {
            float halfDuration = _flashingDuration / 2;
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