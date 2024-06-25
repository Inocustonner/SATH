using System;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class PlayerBullet : MonoBehaviour, IPoolEntity
    {
        [Header("Components")] 
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private Rigidbody2D _rigidbody; 
        
        [Header("Static data")]
        private Transform _player;
        private float _speed;
        
        public int Damage { get; private set; }
        public event Action OnTouched;
        
        public void InitEntity(params object[] parameters)
        {
            Damage = (int)parameters[0];
            _speed = (float)parameters[1];
        }

        public void EnableEntity()
        {
            _collisionObserver.OnEnter += OnCollision;
        }

        public void DisableEntity()
        {
            _collisionObserver.OnEnter -= OnCollision;
        }

        public void Move()
        {
            _rigidbody.velocity += new Vector2(0, _speed * Time.deltaTime);
        }

        private void OnCollision(GameObject obj)
        {
            OnTouched?.Invoke();    
        }
    }
}