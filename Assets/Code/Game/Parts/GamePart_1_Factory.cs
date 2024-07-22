using System.Collections;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.Components.Factory;
using Code.Game.CustomActions.Actions;
using Code.Infrastructure.Audio.AudioEvents;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Parts
{
    public class GamePart_1_Factory: GamePart, IPartTickListener
    {
        public override GamePartName GamePartName => GamePartName.Part_1__factory;
        
        [Header("Components")]
        [SerializeField] private ReplicaNumerableAction _replicasAction;
        [SerializeField] private PushListener _pushListener;
        [SerializeField] private TransitionNextGamePartAction _nextGamePart;
        [SerializeField] private AudioEvent _endAudio;
        
        [Header("Static value")] 
        [SerializeField] private float _transitionToNextLevelDelay = 1;
        private const int MAX_ID = 13;
        
        [Header("Dynamic value")]
        private int _id;
        private Coroutine _transitionCoroutine;

        public override void Restart()
        {
            _id = 0;
        }

        public void PartTick()
        {
            var pushTime = _pushListener.GetPushTime(); 
            if (pushTime / 10 > _id && _id < MAX_ID)
            {
                _replicasAction.SetID(_id);
                _replicasAction.StartAction();
                _id++;
                
                if (_id == MAX_ID)
                { 
                    _endAudio.PlayAudioEvent();
                    _replicasAction.OnEnd += OnEndLastReplica;
                }
            }

        }

        private void OnEndLastReplica()
        {
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
            }
            _transitionCoroutine = StartCoroutine(TransitionToNextSceneWithDelay());
        }

        private IEnumerator TransitionToNextSceneWithDelay()
        {
            yield return new WaitForSeconds(_transitionToNextLevelDelay);
            _nextGamePart.StartAction();
        }
    }
}