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
        }

        public override void StopAction()
        {
            _inputService.OnPressInteractionKey -= OnPressInteractionKey;
            InvokeEndEvent();
            if (_isPressInteraction)
            {
                
            }
        }

        private void OnPressInteractionKey()
        {
        }
    }
}