using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Code.Infrastructure.Audio.AudioSystem
{
    public class AudioVolumeService : MonoBehaviour, IService, IGameInitListener
    {
        private Bus _musicBus;
        private Bus _effectBus;

        public void GameInit()
        {
            InitBus();    
        }

        #region Bus

        private void InitBus()
        {
            // Path copy from FMOD
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _musicBus.setVolume(0);
            _effectBus = RuntimeManager.GetBus("bus:/Effect");
            _effectBus.setVolume(0);
        }

        public void ChangeEffectVolume(float volume)
        {
            _effectBus.setVolume(volume);
        }

        public  void ChangeMusicVolume(float volume)
        {
            _musicBus.setVolume(volume);
        }

        #endregion
        
    }
}