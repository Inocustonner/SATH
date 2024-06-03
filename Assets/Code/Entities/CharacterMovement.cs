using System;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Entities
{
    public class CharacterMovement : MonoBehaviour, IGameInitListener, IGameTickListener, IGameFixedTickListener, IRestartable
    {
        [Header("Components")] 
        [SerializeField] private Rigidbody2D _rb;
        
        [Header("Services")] 
        private InputService _inputService;
       
        [Header("Static data")] 
        [SerializeField] private Vector2 _startPosition;
        
        [Header("Dinamyc data")] 
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        private float _maxSpeedChange;
        private float _acceleration;
        private float _deceleration;
        private float _turnSpeed;
        private bool _isPressingKey;
        
        [Header("Debug")] 
        [SerializeField] private MovementData _data;

        public void GameInit()
        {
            _data = Container.Instance.FindConfig<CharacterConfig>().Movement;
            _inputService = Container.Instance.FindService<InputService>();
        }

        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _isPressingKey = _inputService.GetDirection() != Vector2.zero;
            _desiredVelocity = _inputService.GetDirection() * Mathf.Max(_data.MaxSpeed - _data.Friction, 0f);
        }

        public void GameFixedTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            _velocity = _rb.velocity;
            RunWithAcceleration();
        }

        public void Restart()
        {
            _desiredVelocity = Vector2.zero;
            _velocity = Vector2.zero;
            _rb.velocity = Vector2.zero;
            _isPressingKey = false;
            transform.position = _startPosition;
            this.Log("Restart");
        }

        private void RunWithAcceleration()
        {
            if (_isPressingKey)
            {
                if (Mathf.Sign(_inputService.GetDirection().x) != Mathf.Sign(_velocity.x))
                {
                    _maxSpeedChange = _data.MaxTurnSpeed * Time.deltaTime;
                }
                else
                {
                    _maxSpeedChange = _data.MaxAcceleration * Time.deltaTime;
                }
            }
            else
            {
                _maxSpeedChange = _data.MaxDeceleration * Time.deltaTime;
            }

            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
            _velocity.y = Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, _maxSpeedChange);
        
            _rb.velocity = _velocity;
        }
    }
}