using System;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Services
{
    public class InputService : MonoBehaviour, IService
    {
        private Vector2 _direction;
        private KeyCode _interactionKey = KeyCode.Space;
    
        public event Action OnPressInteraction;

        private void Update()
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.y = Input.GetAxisRaw("Vertical");
       
            if (Input.GetKeyDown(_interactionKey))
            {
                OnPressInteraction?.Invoke();
            }
        }

        public Vector2 GetDirection()
        {
            return _direction;
        }
    }
}
