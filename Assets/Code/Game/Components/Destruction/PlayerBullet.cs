using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class PlayerBullet : Bullet
    {
        [Header("Components")] 
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private Rigidbody2D _rigidbody; 
        
        [Header("Static data")]
        private Transform _player;
        private float _speed;
        
        public int Damage { get; private set; }

        public override  void InitEntity(params object[] parameters)
        {
            Damage = (int)parameters[0];
            _speed = (float)parameters[1];
        }

        public override void EnableEntity()
        {
            _collisionObserver.OnEnter += OnCollision;
        }

        public override void DisableEntity()
        {
            _collisionObserver.OnEnter -= OnCollision;
        }

        public override void Move()
        {
            _rigidbody.velocity += new Vector2(0, _speed * Time.deltaTime);
        }

        private void OnCollision(GameObject obj)
        {
            InvokeTouchedEvent(obj);
        }
    }
}