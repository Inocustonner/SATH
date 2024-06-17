using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using Code.Utils;
using UnityEngine;

namespace Code.Game.CustomActions.Executors
{
    public class SequenceActionExecutor: MonoBehaviour, IPartStartListener, IPartExitListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction _eventAction;
        [SerializeField] private CustomAction[] _nextActions;

        [Header("Params")] 
        [SerializeField] private bool _isPlayOnEndEventAction = true;

        public void PartStart()
        {
            SubscribeToEvents(true);        
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (_eventAction == null)
            {
                this.LogError($"{gameObject.name} check event action");
            }
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