using UnityEngine;

namespace Code.Entities
{
    public class Ball: MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ObjectMover _mover;
        /*[Header("Components")]
        private */
        
        public void SetColliderState(bool isEnabled)
        {
            _collider.enabled = isEnabled;
        }
        
        public void SetPhysicState(bool isEnabled)
        {
            _rigidbody.isKinematic = !isEnabled;
        }
    }
}