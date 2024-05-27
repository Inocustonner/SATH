using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI.Hud.SettingsMenu.Audio
{
    [Serializable]
    public class AudioView
    {
        [SerializeField] private Slider _sliderMusicVolume;
        [SerializeField] private Slider _sliderEffectVolume;
        [SerializeField] private Button _toggleMute;
        [SerializeField] private Image _imageMute;

        public event Action<bool> OnSwitchToggleAudio;
        public event Action<float> OnChangeMusicVolume;
        public event Action<float> OnChangeEffectVolume;

        private bool _isEnabledAudio;

        public void Init(float musicVolume, float effectVolume, bool isEnabledAudio)
        {
            _sliderMusicVolume.value = musicVolume;
            _sliderEffectVolume.value = effectVolume;
            _isEnabledAudio = isEnabledAudio;
            RefreshComponents();
            
            _sliderMusicVolume.onValueChanged.AddListener(ChangeMusicVolume);
            _sliderEffectVolume.onValueChanged.AddListener(ChangeEffectVolume);
            _toggleMute.onClick.AddListener(PressEnabledAudioButton);
        }

        private void ChangeMusicVolume(float sliderValue)
        {
            OnChangeMusicVolume?.Invoke(sliderValue);
        }

        private void ChangeEffectVolume(float sliderValue)
        {
            OnChangeEffectVolume?.Invoke(sliderValue);
        }

        private void PressEnabledAudioButton()
        {
            _isEnabledAudio = !_isEnabledAudio;
            RefreshComponents();
            OnSwitchToggleAudio?.Invoke(_isEnabledAudio);
        }

        private void RefreshComponents()
        {
            _imageMute.color = _isEnabledAudio ? Color.green : Color.red;
            _sliderEffectVolume.interactable = _isEnabledAudio;
            _sliderMusicVolume.interactable = _isEnabledAudio;
        }
  
    }
}