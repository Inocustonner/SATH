using Code.Data.Configs;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Entities
{
    public class CharacterMovement : MonoBehaviour, IGameInitListener, IGameTickListener, IGameFixedTickListener
    {
        [Header("Components")] 
        [SerializeField] private Rigidbody2D _rb;
        [Header("Services")] 
        private InputService _inputService;
        private MoveLimiter _moveLimiter;
        [Header("Debug")] 
        [SerializeField] private MovementData _data;
  
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        private float _maxSpeedChange;
        private float _acceleration;
        private float _deceleration;
        private float _turnSpeed;
        private bool _pressingKey;

        public void GameInit()
        {
            _data = Container.Instance.FindConfig<CharacterConfig>().Movement;
            _inputService = Container.Instance.FindService<InputService>();
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
        }

        public void GameTick()
        {
            if (!gameObject.activeSelf || !_moveLimiter.IsCanMove)
            {
                _rb.velocity = Vector2.zero;
                _desiredVelocity = Vector2.zero;
                return;
            }

            _pressingKey = _inputService.GetDirection() != Vector2.zero;
            _desiredVelocity = _inputService.GetDirection() * Mathf.Max(_data.MaxSpeed - _data.Friction, 0f);
        }

        public void GameFixedTick()
        {
            if (!gameObject.activeSelf || !_moveLimiter.IsCanMove)
            {
                return;
            }
            
            _velocity = _rb.velocity;
            RunWithAcceleration();
        }

        private void RunWithAcceleration()
        {
            if (_pressingKey)
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