using System;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class Enemy : MonoBehaviour,IPoolEntity
    {
        [SerializeField] private CollisionObserver _body;
        [SerializeField] private Health _health;

        [Header("Static data")]
        private Transform _target;
        private float _defaultDistance;
        private float _accelerationSpeed;
        private float _speed;
        private float _gameSpeed;
        public int Damage { get; private set; }

        public event Action OnTakeDamage;
        public event Action OnDeath;

        private Vector3 _startPosition;

        public void InitEntity(params object[] parameters)
        {
            _gameSpeed = (float)parameters[0];
            _defaultDistance = (float)parameters[1];
            _health = new Health((int)parameters[2]);
            _target = (Transform)parameters[3];
            Damage = (int)parameters[4];
        }

        public void EnableEntity()
        {
            SubscribeToEvents(true);
        }

        public void DisableEntity()
        {
            SubscribeToEvents(false);
        }

        public void SetStageParam(Vector3 startPosition,float speed,float acceleration)
        {
            _startPosition = startPosition;
            _speed = speed;
            _accelerationSpeed = acceleration;
        }
        
        public void Move()
        {
            var currentSpeed = _gameSpeed * Vector3.Distance(_startPosition, _target.position) < _defaultDistance
                ? _speed
                : _speed + _accelerationSpeed;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, currentSpeed * Time.deltaTime);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _body.OnEnter += OnEnterBody;
                _health.OnDeath += OnHealthDeath; 
            }
            else
            {
                _body.OnEnter -= OnEnterBody;
                _health.OnDeath -= OnHealthDeath; 
            }
        }
        private void OnHealthDeath()
        {
            OnDeath?.Invoke();
        }

        private void OnEnterBody(GameObject obj)
        {
            if (obj.TryGetComponent(out CharacterMovement character))
            {
                OnTakeDamage?.Invoke();    
                OnDeath?.Invoke();
            }

            if (obj.TryGetComponent(out PlayerBullet bullet))
            {
                _health.GetDamage(bullet.Damage);
            }
        }
    }
}