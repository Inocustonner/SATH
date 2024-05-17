using System;
using UnityEngine;

namespace Code.Entities
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CollisionObserver : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        
        public event Action<GameObject> OnEnter;
        public event Action<GameObject> OnExit;

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnEnter?.Invoke(col.gameObject);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            OnExit?.Invoke(col.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            OnEnter?.Invoke(col.gameObject);
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            OnExit?.Invoke(col.gameObject);
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider2D>();
            }

            Gizmos.color = new Color32(0, 255, 0, 30);
            Gizmos.DrawCube(transform.position + (Vector3)_collider.offset, _collider.size);
        }
    }
}