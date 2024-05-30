using System;
using System.Collections;
using Code.Entities;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services.Materials;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class GlowAction : CustomAction, IGameInitListener
    {
        [SerializeField] private float _flashingDuration = 2;
        [SerializeField, Range(0, 100)] private float _maxGlowValue = 75;
        
        private GlowMaterialController _material;

        private bool _isShine;
        private Coroutine _coroutine;

        public void GameInit()
        {
            _material = Container.Instance.FindService<GlowMaterialController>();
        }

        public override void StartAction()
        {
            _isShine = true;
            TryStopCoroutine();
            _coroutine = StartCoroutine(StartFlashing());
            InvokeStartEvent();
        }

        public override void StopAction()
        {
            _isShine = false;
            TryStopCoroutine();
            if (gameObject.activeInHierarchy)
            {
                _coroutine = StartCoroutine(StopFlashing(onEnd: InvokeEndEvent));
            }
            else
            {
                _material.SetFloatValue(MaterialFloatValueType._Glow,0);
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

            while (_isShine)
            {
                float elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(MaterialFloatValueType._Glow,
                        Mathf.Lerp(0, _maxGlowValue, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(MaterialFloatValueType._Glow, 100);
                elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    _material.SetFloatValue(MaterialFloatValueType._Glow,
                        Mathf.Lerp(_maxGlowValue, 0, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _material.SetFloatValue(MaterialFloatValueType._Glow, 0);
            }
        }

        private IEnumerator StopFlashing(Action onEnd)
        {
            float halfDuration = _flashingDuration / 2;
            float elapsedTime = 0.0f;
            float currentVaLue = _material.GetFloatValue(MaterialFloatValueType._Glow);
            
            while (elapsedTime < halfDuration && _material.GetFloatValue(MaterialFloatValueType._Glow) > 0)
            {
                _material.SetFloatValue(MaterialFloatValueType._Glow,
                    Mathf.Lerp(currentVaLue, 0, elapsedTime / halfDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _material.SetFloatValue(MaterialFloatValueType._Glow, 0);
            onEnd?.Invoke();
        }
    }
}