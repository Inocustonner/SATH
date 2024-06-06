using System;
using System.Collections;
using Code.Data.Configs;
using Code.Data.DynamicData;
using Code.Data.Interfaces;
using Code.Infrastructure.Audio.AudioEvents;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Code.UI.Components
{
    public class AnimatedText : MonoBehaviour, IGameInitListener, IGameStartListener, IGameExitListener
    {
        public bool IsTyping { get; private set; }
        
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;
        [SerializeField] private AudioEvent _audioEvent;

        [Header("Services")]
        private AnimatedTextWaiter _animatedTextWaiter;
        private TextLimiter _textLimiter;
        
        [Header("Static data")]
        private float _defaultSpeed;
        
        [Header("Dinamyc data")]
        private int _index;
        private AcceleratedTextData[] _acceleratedTexts;
        private Coroutine _coroutine;

        public event Action OnEndWrite;
        
        public void GameInit()
        {
            _textLimiter = Container.Instance.FindService<TextLimiter>();
            _animatedTextWaiter = Container.Instance.FindService<AnimatedTextWaiter>();
            
            var uiConfig = Container.Instance.FindConfig<UIConfig>();
            _defaultSpeed = uiConfig.DefaultTypingSpeed;
            
            _textAnimatorPlayer.onCharacterVisible.AddListener(PlayTypeAudio);
            _textAnimatorPlayer.onTypewriterStart.AddListener(() => IsTyping = true);
            _textAnimatorPlayer.onTextShowed.AddListener(() =>
            {
                if (_acceleratedTexts != null && _index < _acceleratedTexts.Length - 1)
                {
                    _text.SetText(_acceleratedTexts[_index].Text);
                    _coroutine ??= StartCoroutine(WaitWhenCanStartWriteNext());
                }
                else
                {
                    _coroutine ??= StartCoroutine(WaitDelayAfterEnd());
                }
            });
        }
        
        public void GameStart()
        {
            _textLimiter.OnSwitch += OnSwitch;  
        }

        public void GameExit()
        {
            _textLimiter.OnSwitch -= OnSwitch;  
        }

        private  void PlayTypeAudio(char c)
        {
            if (c == ' ')
            {
                //todo звук печатанья пробела
            }
            else
            {
                _audioEvent.PlayAudioEvent();
            }
        }

        private void OnDisable()
        {
            TryStopCoroutine();
            Skip();
        }

        public void ResetText()
        {
            _text.SetText("");
            _index = 0;
            _acceleratedTexts = null;
        }

        public void Skip()
        {
            if (_animatedTextWaiter.IsReadySkip)
            {
                _textAnimatorPlayer.SkipTypewriter();
            }
        }

        public void StartWrite(AcceleratedTextData[] replicas, AnimatedTextWaiter.Mode waitedMode)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _index = 0;
            _acceleratedTexts = replicas;
            _animatedTextWaiter.SetMode(waitedMode);
            var currentReplica = replicas[_index];
            StartWrite(currentReplica.Text, currentReplica.Speed);
        }

        private void StartWriteNext()
        {
            _index++;
            var currentReplica = _acceleratedTexts[_index];
            StartWrite(currentReplica.Text, currentReplica.Speed);
        }

        private void StartWrite(string text, float speed)
        {
            _animatedTextWaiter.ResetSkipDelay();
            _animatedTextWaiter.Reset();
            _textAnimatorPlayer.waitForNormalChars = speed > 0 ? speed : _defaultSpeed;
            _textAnimatorPlayer.ShowText(text);
        }

        private IEnumerator WaitWhenCanStartWriteNext()
        {
            _animatedTextWaiter.StartWait();
            yield return new WaitUntil(() => _animatedTextWaiter.IsReadyDelay);
            StartWriteNext();
            _coroutine = null;
        }

        private IEnumerator WaitDelayAfterEnd()
        {
            _animatedTextWaiter.Reset();
            _animatedTextWaiter.SetMode(AnimatedTextWaiter.Mode.Time);
            _animatedTextWaiter.StartWait();
            yield return new WaitUntil(() => _animatedTextWaiter.IsReadyDelay);
            IsTyping = false;
            OnEndWrite?.Invoke();
            _coroutine = null;
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
        
        private void OnSwitch(bool isUnlock)
        {
            if (IsTyping)
            {
                if (isUnlock)
                {
                    _textAnimatorPlayer.ResumeTypewriter();
                    
                }
                else
                {
                    _textAnimatorPlayer.PauseTypewriter();
                }
            }
        }
    }
}