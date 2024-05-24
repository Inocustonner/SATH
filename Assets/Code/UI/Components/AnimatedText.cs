using System;
using System.Collections;
using Code.Data.Configs;
using Code.Data.DynamicData;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class AnimatedText : MonoBehaviour, IGameInitListener
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;
        [Header("Services")]
        private AnimatedTextWaiter _animatedTextWaiter;
        [Header("Static data")]
        private float _defaultSpeed;
        [Header("Dinamyc data")]
        private int _index;
        private AcceleratedText[] _acceleratedTexts;
        private Coroutine _coroutine;
        public bool IsTyping { get; private set; }
        public event Action OnEndWrite;
        
        public void GameInit()
        {
            _animatedTextWaiter = Container.Instance.FindService<AnimatedTextWaiter>();
            
            var uiConfig = Container.Instance.FindConfig<UIConfig>();
            _defaultSpeed = uiConfig.TypingSpeed;
            
            _textAnimatorPlayer.onCharacterVisible.AddListener(PlayTypeAudio);
            _textAnimatorPlayer.onTypewriterStart.AddListener(() => IsTyping = true);
            _textAnimatorPlayer.onTextShowed.AddListener(() =>
            {
                if (_acceleratedTexts != null && _index < _acceleratedTexts.Length - 1)
                {
                    this.Log("1");
                    _text.SetText(_acceleratedTexts[_index].Text);
                    TryStopCoroutine();
                    _coroutine = StartCoroutine(WaitWhenCanStartWriteNext());
                }
                else
                {
                    this.Log("2");
                    IsTyping = false;
                    ResetText();
                    OnEndWrite?.Invoke();
                }
            });
        }

        private static void PlayTypeAudio(char c)
        {
            if (c == ' ')
            {
                //todo звук печатанья пробела
            }
            else
            {
                //todo звук печатанья 
            }
        }

        private void OnDisable()
        {
            TryStopCoroutine();
            StopWrite();
        }

        public void ResetText()
        {
            _text.SetText("");
            _index = 0;
            _acceleratedTexts = null;
        }

        public void StartWrite(AcceleratedText[] replicas, AnimatedTextWaiter.Mode waitedMode)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _index = 0;
            _acceleratedTexts = replicas;
            _animatedTextWaiter.Reset();
            _animatedTextWaiter.SetMode(waitedMode);
            var currentReplica = replicas[_index];
            StartWrite(currentReplica.Text, currentReplica.Speed);
        }

        public void StartWrite(string text, float speed)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _animatedTextWaiter.Reset();
            _textAnimatorPlayer.waitForNormalChars = speed > 0 ? speed : _defaultSpeed;
            _textAnimatorPlayer.ShowText(text);
        }

        public void StopWrite()
        {
            this.Log("stop write");
            _textAnimatorPlayer.SkipTypewriter();
        }
        
        private IEnumerator WaitWhenCanStartWriteNext()
        {
            _animatedTextWaiter.StartWait();
            yield return new WaitUntil(() => _animatedTextWaiter.IsReady);
            StartWriteNext();
        }

        private void StartWriteNext()
        {
            _index++;
            var currentReplica = _acceleratedTexts[_index];
            StartWrite(currentReplica.Text, currentReplica.Speed);
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}