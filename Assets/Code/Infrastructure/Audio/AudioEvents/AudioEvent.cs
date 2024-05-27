using System;
using FMODUnity;
using UnityEngine;

namespace Code.Audio.AudioEvents
{
    [Serializable]
    public class AudioEvent
    {
        [SerializeField] private EventReference _eventReference;
        [SerializeField] private bool _playOnAwake;
        private void OnEnable()
        {
            if (_playOnAwake)
            {
                PlayAudioEvent();
            }
        }

        public void SetEventReference(EventReference eventReference)
        {
            _eventReference = eventReference;
        }
        public void PlayAudioEvent()
        {
            if (_eventReference.IsNull)
            {
                return;
            }

            RuntimeManager.PlayOneShot(_eventReference);
        }

        public void PlayAudioEvent(EventReference eventReference)
        {
            if (eventReference.IsNull)
            {
                return;
            }

            RuntimeManager.PlayOneShot(eventReference);
        }
    }
}