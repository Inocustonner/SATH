using UnityEngine;

namespace Code.Infrastructure.Audio.AudioEvents
{
    public class RepeatAudioEvent : MonoBehaviour
    {
        [SerializeField] private FMODUnity.EventReference _audioPath;
        [SerializeField] private float _repeatDelay = 5;
        [SerializeField] private float _delayBeforeStarting = 15;
        
        private bool _isPlaying;

        private void OnEnable()
        {
            PlayAudio();
        }

        private void OnDisable()
        {
            StopAudio();
        }

        private void PlayAudio()
        {
            /*if(_audioPath.Path == string.Empty)
                return;*/
            
            _isPlaying = true;
          //  RepeatAudio().Forget();
        }

        private void StopAudio()
        {
            /*if(_audioPath.Path == string.Empty)
                return;*/
            
            _isPlaying = false;
        }

        /*private async UniTaskVoid RepeatAudio()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeStarting));
            
            while (_isPlaying)
            {
                FMODUnity.RuntimeManager.PlayOneShot(_audioPath.Guid, transform.position);
             await UniTask.Delay(TimeSpan.FromSeconds(_repeatDelay));
             
            }
        }*/
    }
}