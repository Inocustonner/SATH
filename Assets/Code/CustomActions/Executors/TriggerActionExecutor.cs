using Code.CustomActions.Actions;
using Code.Entities;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class TriggerActionExecutor : MonoBehaviour
    {
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private CustomAction[] _customActions;
        [Header("Debug")] 
        [SerializeField] private bool _isOnTrigger;
        private void Awake()
        {
            SubscribeToEvents(true);
        }

        private void OnDestroy()
        {
            SubscribeToEvents(false);
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
            foreach (var action in _customActions)
            {
                action.StartAction();
            }
            _isOnTrigger = true;
        }

        private void OnExit(GameObject obj)
        {
            foreach (var action in _customActions)
            {
                action.StopAction();
            }
            _isOnTrigger = false;
        }
    }
}