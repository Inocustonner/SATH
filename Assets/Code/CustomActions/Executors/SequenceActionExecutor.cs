using Code.CustomActions.Actions;
using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class SequenceActionExecutor: MonoBehaviour, IGameStartListener, IGameExitListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction _eventAction;
        [SerializeField] private CustomAction[] _nextActions;

        [Header("Params")] 
        [SerializeField] private bool _isPlayOnEndEventAction = true;

        public void GameStart()
        {
            SubscribeToEvents(true);        
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                if (_isPlayOnEndEventAction)
                {
                    _eventAction.OnEnd += OnEndEventAction;
                }
                else
                {
                    _eventAction.OnStart += OnStartEventAction;
                }
            }
            else
            {
                if (_isPlayOnEndEventAction)
                {
                    _eventAction.OnEnd -= OnEndEventAction;
                }
                else
                {
                    _eventAction.OnStart -= OnStartEventAction;
                }
            }
        }

        private void OnStartEventAction()
        {
            StartNextActions();
        }

        private void OnEndEventAction()
        {
            StartNextActions();
        }

        private void StartNextActions()
        {
            foreach (var action in _nextActions)
            {
                action.StartAction();
            }
        }
    }
}