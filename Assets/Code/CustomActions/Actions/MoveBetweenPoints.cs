using System;
using System.Collections;
using Code.CustomActions;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Code.Entities
{
    public class MoveBetweenPoints : CustomAction
    {
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
            _originalPosition = transform.position;
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
                transform.position = Vector3.Lerp(transform.position, to, _speed * Time.deltaTime);
                _currentDistance = Vector3.Distance(transform.position, to);
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
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(_distance,0,0));
        }
    }
}