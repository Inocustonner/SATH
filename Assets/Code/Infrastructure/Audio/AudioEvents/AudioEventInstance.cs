using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Code.Audio.AudioEvents
{
    public class AudioEventInstance : MonoBehaviour
    {
        [SerializeField] private EventReference _event;
        private EventInstance _instance;
        
        private void OnEnable()
        {
            PlayEvent();
        }

        private void OnDisable()
        {
            _instance.stop(STOP_MODE.ALLOWFADEOUT);
        }

        
        private void PlayEvent()
        {
            if(_event.IsNull)
                return;
            
            _instance = RuntimeManager.CreateInstance(_event);
            _instance.start();
        }

    }
}