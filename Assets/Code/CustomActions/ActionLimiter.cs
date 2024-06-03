using System.Collections;
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
        [SerializeField] private float _unlockDelay;
        [SerializeField] private bool _isLockInteraction = true;
        [SerializeField] private bool _isLockMovement = true;
        
        [Header("Services")] 
        private InteractionLimiter _interactionLimiter;
        private MoveLimiter _moveLimiter;

        [Header("Services")] 
        private Coroutine _coroutine;
        
        
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
            this.Log($"Start {_customAction.gameObject.name}",Color.black);
            Block();
        }

        private void OnEndCustomAction()
        {
            if (_unlockDelay == 0)
            {
                Unblock();
            }
            else
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    Unblock();
                }
                _coroutine = StartCoroutine(UnblockWithDelay());
            }
        }

        private void Block()
        {
            if (_isLockInteraction)
            {
                _interactionLimiter.Block();
            }

            if (_isLockMovement)
            {
                _moveLimiter.Block();
            }
        }

        private void Unblock()
        {
            if (_isLockInteraction)
            {
                _interactionLimiter.Unblock();
            }

            if (_isLockMovement)
            {
                _moveLimiter.Unblock();
            }
            this.Log($"End {_customAction.gameObject.name}",Color.black);
        }

        private IEnumerator UnblockWithDelay()
        {
            yield return new WaitForSeconds(_unlockDelay);
            Unblock();
        }
    }
}