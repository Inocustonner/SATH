using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Entities
{
    public class Ball: MonoBehaviour, IGameTickListener
    {
        [Header("Ball Components")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;

        [Header("Scene Components")] 
        [SerializeField] private Transform _teacher;

        [Header("Params")] 
        [SerializeField] private bool _isFollow;


        public void GameTick()
        {
            if (_isFollow)
            {
                transform.position = Vector3.Lerp(transform.position, _teacher.position + new Vector3(0,1.5f,0), 15 * Time.deltaTime);
            }
        }

        public void SwitchFollow(bool isFollow)
        {
            _isFollow = !_isFollow;
            SetPhysicState(!_isFollow);
            SetColliderState(!_isFollow);
        }

        private void SetPhysicState(bool isEnabled)
        {
            _rigidbody.isKinematic = !isEnabled;
        }

        public void SetColliderState(bool isEnabled)
        {
            _collider.enabled = isEnabled;
        }
    }
}