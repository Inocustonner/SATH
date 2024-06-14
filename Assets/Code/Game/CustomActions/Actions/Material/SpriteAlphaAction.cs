using System;
using System.Collections;
using Code.Data.Interfaces;
using Code.Data.Values.RangeFloat;
using UnityEngine;

namespace Code.Game.CustomActions.Actions.Material
{
    public class SpriteAlphaAction : CustomAction,IPartStartListener, IPartExitListener
    {
        [SerializeField,MinMaxRangeFloat(0, 10)] private RangedFloat _flashingDuration;
        [SerializeField, MinMaxRangeFloat(0, 1)] private RangedFloat _alphaRange;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private bool _isActive;
        private Coroutine _coroutine;
        
        public void PartStart()
        {
            SetAlpha(1);
        }

        public void PartExit()
        {
            SetAlpha(1);
        }

        public override void StartAction()
        {
            _isActive = true;
            TryStopCoroutine();
            _coroutine = StartCoroutine(StartFlashing());
            InvokeStartActionEvent();
        }

        private void SetAlpha(float value)
        {
            var color = _spriteRenderer.color;
            color.a = value;
            _spriteRenderer.color = color;
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
                SetAlpha(1);
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
                float maxAlpha = _alphaRange.MaxValue;
                float minAlpha = _alphaRange.MinValue;
                float elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    SetAlpha(Mathf.Lerp(minAlpha, maxAlpha, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                SetAlpha(maxAlpha);
                elapsedTime = 0.0f;

                while (elapsedTime < halfDuration)
                {
                    SetAlpha(Mathf.Lerp(maxAlpha,minAlpha, elapsedTime / halfDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                SetAlpha(minAlpha);
            }
        }

        private IEnumerator StopFlashing(Action onEnd)
        {
            float halfDuration = _flashingDuration.GetRandomValue() / 2;
            float elapsedTime = 0.0f;
            float currentVaLue = _spriteRenderer.color.a;
            float minValue = _alphaRange.MinValue;
            
            while (elapsedTime < halfDuration &&  _spriteRenderer.color.a > minValue)
            {
               SetAlpha(Mathf.Lerp(currentVaLue, minValue, elapsedTime / halfDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetAlpha(minValue);
            onEnd?.Invoke();
        }
    }
}