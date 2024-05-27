using FMODUnity;
using UnityEngine;

namespace Code.Audio.AudioEvents
{
    public class AudioEventComponent : MonoBehaviour
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

        public void PlayAudioEvent()
        {
            if (_eventReference.IsNull)
            {
                return;
            }

            RuntimeManager.PlayOneShot(_eventReference, gameObject.transform.position);
        }
    }
}