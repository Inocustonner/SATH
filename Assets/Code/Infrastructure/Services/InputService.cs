using System;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class InputService :  IService, IGameTickListener
    {
        private Vector2 _direction;
        private KeyCode _interactionKey = KeyCode.E;
    
        public event Action OnPressInteractionKey;

        public void GameTick()
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.y = Input.GetAxisRaw("Vertical");
       
            if (Input.GetKeyDown(_interactionKey))
            {
                OnPressInteractionKey?.Invoke();
            }
        }
        
        public Vector2 GetDirection()
        {
            return _direction;
        }
    }
}
