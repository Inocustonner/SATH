using System.Collections;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.Components.Factory;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.Parts
{
    public class GamePart_2_BadEndingLove : GamePart,IGameInitListener, IPartTickListener, IRestarable
    {
        public override GamePartName GamePartName => GamePartName.Part_2__bad_ending_love;
        [Header("Components")]
        [SerializeField] private ReplicaNumerableAction _replicasAction;
        [SerializeField] private PushListener _pushListener;
        [SerializeField] private TransitionNextGamePartAction _nextGamePart;

        [Header("Static value")] 
        [SerializeField] private float _transitionToNextLevelDelay = 1;
        private int _replicasCount;

        [Header("Dynamic value")]
        private int _id;
        private Coroutine _transitionCoroutine;

        public void GameInit()
        {
            _replicasCount = _replicasAction.GetReplicasCount();
        }

        public void PartTick()
        {
            var pushTime = _pushListener.GetPushTime(); 
            if (pushTime / 10 > _id && _id < _replicasCount)
            {
                _replicasAction.SetID(_id);
                _replicasAction.StartAction();
                _id++;
                
                if (_id == _replicasCount)
                {
                    _replicasAction.OnEnd += OnEndLastReplica;
                }
            }

        }

        public override void Restart()
        {
            _id = 0;
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