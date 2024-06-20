using System;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class InputService :  IService, IGameInitListener,IGameTickListener
    {
        [Header("Services")]
        private MoveLimiter _moveLimiter;
        private InteractionLimiter _interactionLimiter;

        [Header("Dinamyc data")]
        private Vector2 _direction;
        private KeyCode _interactionKey = KeyCode.E;
        private KeyCode _pauseKey = KeyCode.Escape;

        public event Action OnPressInteractionKey;
        public event Action OnPressPauseKey;
        public event Action<Vector2> OnPressMove;

        public void GameInit()
        {
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _interactionLimiter = Container.Instance.FindService<InteractionLimiter>();
        }

        public void GameTick()
        {
            if (Input.GetKeyDown(_pauseKey))
            {
                OnPressPauseKey?.Invoke();
            }

            CheckDirection();
            CheckInteraction();
        }

        public Vector2 GetDirection()
        {
            return _direction;
        }

        private void CheckInteraction()
        {
            if (_interactionLimiter.IsUnlock && Input.GetKeyDown(_interactionKey))
            {
                OnPressInteractionKey?.Invoke();
            }
        }

        private void CheckDirection()
        {
            if (!_moveLimiter.IsUnlock)
            {
                _direction = Vector2.zero;
            }
            else
            {
                var dir = new Vector2
                {
                    x = Input.GetAxisRaw("Horizontal"),
                    y = Input.GetAxisRaw("Vertical")
                };
                
                if (dir != _direction)
                {
                    OnPressMove?.Invoke(dir);
                }

                _direction = dir;
            }
        }
    }
}
