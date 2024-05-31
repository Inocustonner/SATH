using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class WaitInteractionPressAction: CustomAction, IGameInitListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction[] _waitingActions;
        [SerializeField] private CustomAction[] _resultActions;

        [Header("Services")]
        private InputService _inputService;

        [Header("Dimanyc value")] 
        private bool _isPressInteraction;
        
        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
        }

        public override void StartAction()
        {
            _inputService.OnPressInteractionKey += OnPressInteractionKey;

            foreach (var waitingAction in _waitingActions)
            {
                waitingAction.StartAction();
            }
        }

        public override void StopAction()
        {
            _inputService.OnPressInteractionKey -= OnPressInteractionKey;
            
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
            
            InvokeStartEvent();
            InvokeEndEvent();
        }

        private void OnPressInteractionKey()
        {
            _isPressInteraction = true;
            StopAction();
        }
    }
}