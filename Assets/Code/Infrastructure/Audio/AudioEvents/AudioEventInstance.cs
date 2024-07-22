using Code.Utils;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Code.Infrastructure.Audio.AudioEvents
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
            StopEvent();
        }
        
        [ContextMenu("Play")]
        private void PlayEvent()
        {
            if (_event.IsNull)
            {
                this.LogError($"{gameObject.name} event reference is null");
                return;
            }
            
            this.Log($"Play {_event.Path}");
            _instance = RuntimeManager.CreateInstance(_event);
            _instance.set3DAttributes(transform.position.To3DAttributes());
            _instance.start();
            _instance.release();
        }

        private void StopEvent()
        {
            _instance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}