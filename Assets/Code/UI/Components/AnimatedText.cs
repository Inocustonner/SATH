using System;
using Code.Data.DynamicData;
using Code.Infrastructure.GameLoop;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class AnimatedText : MonoBehaviour, IGameInitListener
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;
        private float _defaultSpeed;
        public bool IsTyping { get; private set; }
        public event Action OnEndWrite;

        private int _index;
        private AcceleratedText[] _acceleratedTexts;
        
        public void GameInit()
        {
            _defaultSpeed = _textAnimatorPlayer.waitForNormalChars;
            _textAnimatorPlayer.onCharacterVisible.AddListener(PlayTypeAudio);
            _textAnimatorPlayer.onTypewriterStart.AddListener(() => IsTyping = true);
            _textAnimatorPlayer.onTextShowed.AddListener(() =>
            {
                if (_acceleratedTexts != null && _index < _acceleratedTexts.Length - 1)
                {
                    StartWriteNext();
                }
                else
                {
                    IsTyping = false;
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
            StopWrite();
        }

        public void Reset()
        {
            _text.SetText("");
            _index = 0;
            _acceleratedTexts = null;
        }

        public void StartWrite(string text, float speed)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _textAnimatorPlayer.waitForNormalChars = speed > 0 ? speed : _defaultSpeed;
            _textAnimatorPlayer.ShowText(text);
        }

        public void StartWrite(AcceleratedText[] replicas)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _index = 0;
            _acceleratedTexts = replicas;
            var currentReplica = replicas[_index];
            StartWrite(currentReplica.Text, currentReplica.Speed);
        }

        public void StopWrite()
        {
            _textAnimatorPlayer.SkipTypewriter();
        }

        private void StartWriteNext()
        {
            _index++;
            var currentReplica = _acceleratedTexts[_index];
            StartWrite(currentReplica.Text, currentReplica.Speed);
        }
        
    }
}