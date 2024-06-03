using System.Collections;
using Code.Entities;
using Code.Utils;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class MoveToPointAction: CustomAction
    {
        [SerializeField] private Vector2 _target;
        [SerializeField] private ObjectMover _mover;
        [SerializeField] private Collider2D[] _disableColliders;
        
        private Coroutine _coroutine;

        public override void StartAction()
        {
            TryStopCoroutine();
            InvokeStartEvent();
            SetComponentsEnable(false);
            _coroutine = StartCoroutine(_mover.Move(_target, onCompeted: () =>
            {
                SetComponentsEnable(true);
                InvokeEndEvent();
            }));
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
        
        private void SetComponentsEnable(bool isEnabled)
        {
            foreach (var collider in _disableColliders)
            {
                collider.enabled = isEnabled;
            }    
        }
        
        private void OnDrawGizmosSelected()
        {
            var color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(_target, 0.2f);
        }
    }
}