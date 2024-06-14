using System.Collections;
using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.CustomActions.Executors
{
    public class ReplicaNumerableEndExecutor: MonoBehaviour, IPartStartListener, IPartExitListener
    {
        [SerializeField] private ReplicaNumerableAction _replicaNumerableAction;
        [SerializeField] private float _delay;
        [SerializeField] private CustomAction[] _nextActions;
        private Coroutine _coroutine;

        public void PartStart()
        {
            SubscribeToEvents(true);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
            TryStopCoroutine();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _replicaNumerableAction.OnSetLastReplica += OnSetLastReplica;
            }
            else
            {
                _replicaNumerableAction.OnSetLastReplica -= OnSetLastReplica;
                
            }
        }

        private void OnSetLastReplica()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(StartActionsWithDelay());
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
        
        private IEnumerator StartActionsWithDelay()
        {
            yield return new WaitForSeconds(_delay);
            for (int i = 0; i < _nextActions.Length; i++)
            {
                _nextActions[i].StartAction();
            }
        }
    }
}