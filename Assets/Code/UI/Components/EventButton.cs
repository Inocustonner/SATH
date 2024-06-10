using System;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.Audio.AudioEvents;
using Code.Infrastructure.DI;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    public class EventButton: MonoBehaviour, IGameInitListener,IGameStartListener
    {
        [SerializeField] private Button _button;
        private readonly AudioEvent _audioEvent = new();
        public event Action OnClickButton;
        
        public void GameInit()
        {
            _audioEvent.SetEventReference(Container.Instance.FindConfig<UIConfig>().ClickButtonAudio);
        }

        public void GameStart()
        {
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            _audioEvent.PlayAudioEvent();
            OnClickButton?.Invoke();
        }
    }
}