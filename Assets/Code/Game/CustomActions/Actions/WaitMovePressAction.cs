using System.Collections;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class WaitMovePressAction: CustomAction, IGameInitListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction[] _waitingActions;
        [SerializeField] private CustomAction[] _resultActions;

        [Header("Services")]
        private InputService _inputService;

        [Header("Dimanyc value")] 
        private bool _isPressInteraction;
        private Coroutine _coroutine;
        
        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
        }

        public override void StartAction()
        {
            TryStopCoroutine();

            foreach (var waitingAction in _waitingActions)
            {
                waitingAction.StartAction();
            }

            _coroutine = StartCoroutine(WaitPressMove());
        }

        public override void StopAction()
        {
            foreach (var waitingAction in _waitingActions)
            {
                waitingAction.StopAction();
            }

            if (!_isPressInteraction)
            {
                return;
            }
            
            _isPressInteraction = false;
            foreach (var resultAction in _resultActions)
            {
                resultAction.StartAction();
            }
            
            InvokeStartActionEvent();
            InvokeEndActionEvent();
        }

        private IEnumerator WaitPressMove()
        {
            yield return new WaitUntil(() => _inputService.GetDirection() != Vector2.zero);
            _isPressInteraction = true;
            StopAction();
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}