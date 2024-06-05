using System.Collections;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions
{
    public class MoveBetweenTwoPointsAction : CustomAction, IGameInitListener, IPartExitListener, IRestarable
    {
        [SerializeField] private Transform _object;
        [SerializeField] private float _speed = 2;
        [SerializeField] private float _distance = 1;
        
        private Vector2 _originalPosition;
        private Vector2 _targetPosition => _originalPosition + new Vector2(_distance, 0);
        private Coroutine _coroutine;

        public void GameInit()
        {
            _originalPosition = _object.position;
        }

        public void PartExit()
        {
            TryStopCoroutine();
        }

        public void Restart()
        {
            TryStopCoroutine();
            _object.position = _originalPosition;
        }

        public override void StartAction()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(Move(to: _targetPosition));
        }

        public override void StopAction()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(Move(to: _originalPosition));
        }

        private IEnumerator Move(Vector2 to)
        {
            var period = new WaitForEndOfFrame();
            while (Vector3.Distance(_object.position, to) > 0.01f)
            {
                _object.position = Vector3.Lerp(_object.position, to, _speed * Time.deltaTime);
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