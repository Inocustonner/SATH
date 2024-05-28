using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Code.Infrastructure.Audio.AudioSystem
{
    public class SceneAudioController : MonoBehaviour, IService, IGameInitListener, IGameStartListener
    {
        [SerializeField] private EventReference _musicEvent;
        private EventInstance _music;
        
        private Bus _musicBus;
        private Bus _effectBus;

        public void GameInit()
        {
            InitBus();    
        }

        public void GameStart()
        {
            PlayMusic();    
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

        #region Music

        private void PlayMusic()
        {
            if (_musicEvent.IsNull)
                return;

            _music = RuntimeManager.CreateInstance(_musicEvent);
            _music.start();
        }

        private void StopMusic()
        {
            _music.stop(STOP_MODE.ALLOWFADEOUT);
        }
        

        #endregion
    }
}