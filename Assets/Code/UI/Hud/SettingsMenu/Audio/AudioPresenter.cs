using System;
using Code.Infrastructure.Audio.AudioSystem;
using Code.Infrastructure.DI;
using Code.Utils;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.UI.Hud.SettingsMenu.Audio
{
    [Serializable]
    public class AudioPresenter
    {
        [Header("Conponents")]
        [SerializeField] private AudioView _audioView;
        [Header("Dinamyc data")]
        [SerializeField] private AudioVolumeData _audioVolumeData;
        [Header("Services")]
        private AudioVolumeService _audioVolumeService;
        
        public AudioVolumeData AudioVolumeData => _audioVolumeData;

        public void Init(AudioVolumeService findService)
        {
            _audioVolumeService = findService; 
        }
        
        public void SetValues(AudioVolumeData audioVolumeData)
        {
            _audioVolumeData = audioVolumeData;
            _audioView.Init(audioVolumeData.Music, audioVolumeData.Effects, audioVolumeData.IsEnabled);
            this.Log($"{audioVolumeData.ToJson()}");
            SetAudioValues(audioVolumeData.IsEnabled);
        }

        public void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _audioView.OnChangeEffectVolume += ChangeEffectVolume;
                _audioView.OnChangeMusicVolume += ChangeMusicVolume;
                _audioView.OnSwitchToggleAudio += SetAudioValues;
            }
            else
            {
                _audioView.OnChangeEffectVolume -= ChangeEffectVolume;
                _audioView.OnChangeMusicVolume -= ChangeMusicVolume;
                _audioView.OnSwitchToggleAudio -= SetAudioValues;
            }
        }

        private void SetAudioValues(bool isEnabled)
        {
            _audioVolumeData.IsEnabled = isEnabled;
            if (_audioVolumeData.IsEnabled)
            {
                _audioVolumeService.ChangeEffectVolume(_audioVolumeData.Effects);
                _audioVolumeService.ChangeMusicVolume(_audioVolumeData.Music);
            }
            else
            {
                _audioVolumeService.ChangeEffectVolume(0);
                _audioVolumeService.ChangeMusicVolume(0);
            }
        }

        private void ChangeMusicVolume(float musicVolume)
        {
            _audioVolumeData.Music = musicVolume;
            _audioVolumeService.ChangeMusicVolume(_audioVolumeData.Music);
        }

        private void ChangeEffectVolume(float effectVolume)
        {
            _audioVolumeData.Effects = effectVolume;
            _audioVolumeService.ChangeEffectVolume(_audioVolumeData.Effects);
        }
    }
}