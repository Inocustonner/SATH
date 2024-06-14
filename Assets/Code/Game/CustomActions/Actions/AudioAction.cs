using Code.Infrastructure.Audio.AudioEvents;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class AudioAction: CustomAction
    {
        [SerializeField] private AudioEvent _audioEvent;

        public override void StartAction()
        {
            InvokeStartActionEvent();
            _audioEvent.PlayAudioEvent();
            InvokeEndActionEvent();
        }
    }
}