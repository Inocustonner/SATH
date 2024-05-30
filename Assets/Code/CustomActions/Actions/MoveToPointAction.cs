using System.Collections;
using Code.Utils;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class MoveToPointAction: CustomAction
    {
        [SerializeField] private Transform _object;
        [SerializeField] private Collider2D[] _disableColliders;
        [SerializeField] private float _speed = 2;
        [SerializeField] private Vector2 _target;
        
        private Coroutine _coroutine;
        
        public override void StartAction()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(Move(_target));
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
        
        private IEnumerator Move(Vector2 to)
        {
            SetComponentsEnable(false);
            InvokeStartEvent();
            var period = new WaitForEndOfFrame();
            while (Vector3.Distance(_object.position, to) > 0.1f)
            {
                _object.position = Vector3.Lerp(_object.position, to, _speed * Time.deltaTime);
                yield return period;
            }
            InvokeEndEvent();
            SetComponentsEnable(true);
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
            var color = Constance.PurpleColor;
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(_target, 0.2f);
        }
    }
}