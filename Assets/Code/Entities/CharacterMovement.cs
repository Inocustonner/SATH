using Code.Data.Configs;
using Code.Data.StaticData;
using Code.Services;
using UnityEngine;

namespace Code.Entities
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Services")] 
        [SerializeField] private InputService _inputService;
        [Header("Components")] 
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private CharacterConfig _characterConfig;
        [Header("Debug")] 
        [SerializeField] private MovementData _data;
  
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        private float _maxSpeedChange;
        private float _acceleration;
        private float _deceleration;
        private float _turnSpeed;
        private bool _pressingKey;

        private void Awake()
        {
            _data = _characterConfig.Movement;
        }
    
        private void Update()
        {
            /*
        if (!moveLimit.characterCanMove && !itsTheIntro)
        {
            directionX = 0;
        }
        */

            _pressingKey = _inputService.GetDirection() != Vector2.zero;
            _desiredVelocity = _inputService.GetDirection() * Mathf.Max(_data.MaxSpeed - _data.Friction, 0f);
        }

        private void FixedUpdate()
        {
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