using Code.CustomActions.Actions;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.CustomActions
{
    public class ActionLimiter : MonoBehaviour, IGameInitListener, IGameExitListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction _customAction;
        [Header("Params")]
        [SerializeField] private bool _isLockInteraction;
        [SerializeField] private bool _isLockMovement;
        [Header("Services")] 
        private InteractionLimiter _interactionLimiter;
        private MoveLimiter _moveLimiter;
        
        public void GameInit()
        {
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _interactionLimiter = Container.Instance.FindService<InteractionLimiter>();
            SubscribeToEvents(true); //specifically located here and not at the start. the reason is the class "EnableActionExecutor"
        }
        
        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if(flag)
            {
                _customAction.OnStart += OnStartCustomAction;
                _customAction.OnEnd += OnEndCustomAction;
            }
            else
            {
                _customAction.OnStart -= OnStartCustomAction;
                _customAction.OnEnd -= OnEndCustomAction;
            }
        }

        private void OnStartCustomAction()
        {
            this.Log("Start");
            if (_isLockInteraction)
            {
                _interactionLimiter.Block();
            }

            if (_isLockMovement)
            {
                _moveLimiter.Block();
            }
        }

        private void OnEndCustomAction()
        {
            this.Log("End");
            if (_isLockInteraction)
            {
                _interactionLimiter.Unblock();
            }

            if (_isLockMovement)
            {
                _moveLimiter.Unblock();
            }
        }
    }
}