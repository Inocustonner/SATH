using System;
using UnityEngine;

namespace Code.GameParts.Components
{
    public class CollisionObserver : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        
        public event Action<GameObject> OnEnter;
        public event Action<GameObject> OnExit;

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnEnter?.Invoke(col.gameObject);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (!gameObject.activeSelf || !gameObject.activeInHierarchy)
            {
                return;
            }
            OnExit?.Invoke(col.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            OnEnter?.Invoke(col.gameObject);
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (!gameObject.activeSelf || !gameObject.activeInHierarchy)
            {
                return;
            }
            OnExit?.Invoke(col.gameObject);
        }

        private void OnDrawGizmos()
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
                return;
            }

            if (_collider is BoxCollider2D boxCollider2D)
            {
                Gizmos.color = new Color32(0, 255, 0, 30);
                Gizmos.DrawCube(transform.position + (Vector3)boxCollider2D.offset, boxCollider2D.size);
            }
            else if(_collider is CircleCollider2D circleCollider2D)
            {
                Gizmos.color = new Color32(0, 255, 0, 30);
                Gizmos.DrawSphere(transform.position + (Vector3)circleCollider2D.offset, circleCollider2D.radius);
            }
        }
    }
}