using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class AnimatedText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;
        public bool IsTyping { get; private set; }

        private void Awake()
        {
            _textAnimatorPlayer.onCharacterVisible.AddListener(PlayTypeAudio);
            _textAnimatorPlayer.onTypewriterStart.AddListener(() => IsTyping = true);
            _textAnimatorPlayer.onTextShowed.AddListener(() => IsTyping = false);
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
        }

        public void StartWrite(string text)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
       
            _textAnimatorPlayer.ShowText(text);
        }


        public void StopWrite()
        {
            _textAnimatorPlayer.SkipTypewriter();
        }

    }
}