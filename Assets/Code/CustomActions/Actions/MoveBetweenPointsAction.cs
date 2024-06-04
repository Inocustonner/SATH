using Code.Entities;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class MoveBetweenPointsAction : CustomAction
    {
        [SerializeField] private ObjectMover _objectMover;
        [SerializeField] private Vector2[] _points;
        private int _currentIndex;
        public override void StartAction()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            InvokeStartActionEvent();
            StartCoroutine(_objectMover.Move(_points,onCompeted: StopAction));
        }

        public override void StopAction()
        {
            InvokeEndActionEvent();
        }

        private void OnDrawGizmosSelected()
        {
            if (_points.Length == 0)
            {
                return;
                
            }

            Gizmos.color = Color.green;
            float radius = 0.2f;
            foreach (var point in _points)
            {
                Gizmos.DrawSphere(point, radius);
                radius += 0.2f;
            }
            
        }
    }
}