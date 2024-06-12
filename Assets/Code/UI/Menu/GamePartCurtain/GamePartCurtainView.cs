using System;
using System.Collections;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menu.GamePartCurtain
{
    public class GamePartCurtainView: BaseMenuView, IGameInitListener
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _background;
        private float _duration;

        private Coroutine _coroutine; 
        public void GameInit()
        {
            var config = Container.Instance.FindConfig<UIConfig>();
            _background.color = config.CurtainColor;
            _duration = config.CurtainDuration;
        }

        public override void OpenMenu(Action onComplete = null)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _canvasGroup.alpha = 0;
            windowTransform.gameObject.SetActive(true);
            _coroutine = StartCoroutine(ShowCurtain(onComplete));
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
        }

        private IEnumerator ShowCurtain(Action onComplete)
        {
            float halfDuration = _duration / 2.0f;
            float elapsedTime = 0.0f;

            while (elapsedTime < halfDuration)
            {
                _canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / halfDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 1;
            elapsedTime = 0.0f;

            while (elapsedTime < halfDuration)
            {
                _canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / halfDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 0;
            onComplete?.Invoke();
        }
    }
}