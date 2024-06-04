using Code.CustomActions.Actions;
using Code.Data.Interfaces;
using Code.Entities;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class TriggerActionExecutor : MonoBehaviour,IGameInitListener, IGameExitListener, IResetable
    {
        [Header("Components")] 
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private CustomAction[] _customActions;

        [Header("Params")] 
        [SerializeField] private bool _isStartInsideTrigger = true;
        [SerializeField] private bool _isCanRepeat = true;

        [Header("Dinamyc value")] 
        [SerializeField] private bool _isInvoke; 
        [SerializeField] private bool _isOnTrigger;

        public void GameInit()
        {
            
            SubscribeToEvents(true);
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void OnEnable()
        {
            if (!_isStartInsideTrigger && !_isOnTrigger)
            {
                OnEnter(null);
            }
        }

        public void Restart()
        {
            _isInvoke = false;
            _isOnTrigger = false;
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _collisionObserver.OnEnter += OnEnter;
                _collisionObserver.OnExit += OnExit;
            }
            else
            {
                _collisionObserver.OnEnter -= OnEnter;
                _collisionObserver.OnExit -= OnExit;
            }
        }

        private void OnEnter(GameObject obj)
        {
            if (!_isCanRepeat && _isInvoke)
            {
                return;
            }
            
            _isInvoke = true;
            foreach (var action in _customActions)
            {
                if (_isStartInsideTrigger)
                {
                    action.StartAction();
                }
                else
                {
                    action.StopAction();
                }
            }
            _isOnTrigger = true;
        }

        private void OnExit(GameObject obj)
        {
            if (!_isOnTrigger)
            {
                return;
            }
            foreach (var action in _customActions)
            {
                if (_isStartInsideTrigger)
                {
                    action.StopAction();
                }
                else
                {
                    action.StartAction();
                }
            }
            _isOnTrigger = false;
        }

    
    }
}