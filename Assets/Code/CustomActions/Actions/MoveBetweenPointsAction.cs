using System.Collections;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class MoveBetweenPointsAction : CustomAction
    {
        [SerializeField] private Transform _object;
        [SerializeField] private float _speed = 2;
        [SerializeField] private float _distance = 1;
        [Header("Debug")] 
        [SerializeField] private float _currentDistance;
        [SerializeField] private bool _isMoveToOriginal;
        
        private Vector2 _originalPosition;
        private Vector2 _targetPosition => _originalPosition + new Vector2(_distance, 0);
        private Coroutine _coroutine;

        private void Awake()
        {
            _originalPosition = _object.position;
        }

        private void OnDestroy()
        {
            TryStopCoroutine();
        }
        
        public override void StartAction()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(Move(to: _targetPosition));
            _isMoveToOriginal = false;
        }

        public override void StopAction()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(Move(to: _originalPosition));
            _isMoveToOriginal = true;
        }
        
        private IEnumerator Move(Vector2 to)
        {
            var period = new WaitForEndOfFrame();
            while (Vector3.Distance(transform.position, to) > 0.01f)
            {
                _object.position = Vector3.Lerp(_object.position, to, _speed * Time.deltaTime);
                _currentDistance = Vector3.Distance(_object.position, to);
                yield return period;
            }
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        private void OnDrawGizmos()
        {
            if (_object == null)
            {
                return;
            }
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_object.position, _object.position + new Vector3(_distance,0,0));
        }
    }
}