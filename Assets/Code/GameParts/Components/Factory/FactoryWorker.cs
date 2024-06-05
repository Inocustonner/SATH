using System;
using Code.Data.Interfaces;
using Code.Data.Values.RangeFloat;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.GameParts.Entities
{
    public class FactoryWorker : MonoBehaviour, IPoolEntity
    {
        private const float STOPPED_POSITION_X = -25; 
        
        [SerializeField] private Vector2 _startPosition;
        [SerializeField] private SpriteRenderer _view;
        
        private RangedCooldown _speedChangeCooldown;
        private RangedFloat _speed;

        private float _currentSpeed = 1;
        
        public event Action OnReached;
        public void InitEntity(params object[] parameters)
        {
            _view.color = (Color)parameters[0];
            _speed = (RangedFloat)parameters[1];
            _speedChangeCooldown = new RangedCooldown((RangedFloat)parameters[2]);
        }

        public void EnableEntity()
        {
            transform.position = _startPosition;
            GetTired();
        }

        public void DisableEntity()
        {
            _speedChangeCooldown.Stop();
        }

        public void GetTired()
        {
            _currentSpeed = _speed.GetRandomValue();
            _speedChangeCooldown.Start(isLoop: true, onCompleted: () => _currentSpeed = _speed.GetRandomValue());
        }

        public void Work()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            transform.position -= new Vector3(_currentSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x < STOPPED_POSITION_X)
            {
                OnReached?.Invoke();
            }
        }
    }
}