using Code.Entities;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class TriggerActionExecutor : MonoBehaviour
    {
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private CustomAction _customAction;
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
            _customAction.StartAction();
            _isOnTrigger = true;
        }

        private void OnExit(GameObject obj)
        {
            _customAction.StopAction();
            _isOnTrigger = false;
        }
    }
}