using Code.Data.Interfaces;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Components.School
{
    public class Ball: MonoBehaviour, IPartTickListener, IRestarable
    {
        [Header("Ball Components")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private GameObject _audioObject;

        [Header("Scene Components")] 
        [SerializeField] private Transform _teacher;

        [Header("Params")] 
        [SerializeField] private bool _isFollow;
        
        public void PartTick()
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

        public void Restart()
        {
            SwitchFollow(false);
            this.Log("Restart");
        }

        public void SwitchFollow(bool isFollow)
        {
            _isFollow = isFollow;
            SetPhysicState(!_isFollow);
            SetColliderState(!_isFollow);
            _audioObject.SetActive(!_isFollow);
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