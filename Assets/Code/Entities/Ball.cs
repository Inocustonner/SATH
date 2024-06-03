using Code.Data.Interfaces;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Entities
{
    public class Ball: MonoBehaviour,IGameInitListener ,IGameTickListener, IRestartable
    {
        [Header("Ball Components")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;

        [Header("Scene Components")] 
        [SerializeField] private Transform _teacher;

        [Header("Params")] 
        [SerializeField] private bool _isFollow;
        private Vector3 _startPosition;


        public void GameInit()
        {
            _startPosition = transform.position;
        }

        public void Restart()
        {
            SwitchFollow(false);
            transform.position = _startPosition;
        }

        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            if (_isFollow)
            {
                transform.position = Vector3.Lerp(
                    transform.position, 
                    _teacher.position + new Vector3(-0.5f,1.5f,0), 
                    15 * Time.deltaTime);
            }
        }
        
        public void SwitchFollow(bool isFollow)
        {
            _isFollow = isFollow;
            SetPhysicState(!_isFollow);
            SetColliderState(!_isFollow);
        }

        private void SetPhysicState(bool isEnabled)
        {
            _rigidbody.isKinematic = !isEnabled;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
        }

        private void SetColliderState(bool isEnabled)
        {
            _collider.enabled = isEnabled;
        }
    }
}