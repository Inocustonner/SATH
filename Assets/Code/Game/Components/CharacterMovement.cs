using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Game.Components
{
    public class CharacterMovement : MonoBehaviour, IGameInitListener, IPartTickListener, IPartFixedTickListener, IRestarable
    {
        [Header("Components")] 
        [SerializeField] private Rigidbody2D _rb;
        
        [Header("Services")] 
        private InputService _inputService;

        [Header("Static data")] 
        [SerializeField] private CharacterMovementConfig _data;
        [SerializeField] private bool _isCanVerticalMove = true;

        [Header("Dinamyc data")] 
        private bool _isCanMove = true;
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        private float _maxSpeedChange;
        private float _acceleration;
        private float _deceleration;
        private float _turnSpeed;
        private bool _isPressingKey;
        
        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
        }

        public void PartTick()
        {
            _isPressingKey = _inputService.GetDirection() != Vector2.zero;
            _desiredVelocity = _inputService.GetDirection() * Mathf.Max(_data.MaxSpeed - _data.Friction, 0f);
        }

        public void PartFixedTick()
        {
            _velocity = _rb.velocity;
            RunWithAcceleration();
        }

        public void Restart()
        {
            _desiredVelocity = Vector2.zero;
            _velocity = Vector2.zero;
            _rb.velocity = Vector2.zero;
            _isPressingKey = false;
        }

        public void Block()
        {
            _isCanMove = false;
            Restart();
        }

        public void Unblock()
        {
            _isCanMove = true;
            Restart();
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
            _velocity.y = _isCanVerticalMove ? Mathf.MoveTowards(_velocity.y, _desiredVelocity.y, _maxSpeedChange) : 0;
        
            _rb.velocity = _velocity;
        }
    }
}