using System;
using System.Collections;
using UnityEngine;

namespace Code.GameParts.Components
{
    [Serializable]
    public class ObjectMover
    {
        [SerializeField] private Transform _entity;
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _stoppedDistance = 0.1f;
        
        public Transform Entity => _entity;
        
        public IEnumerator MoveWithAcceleration(Vector2 to, Action onCompeted = null)
        {
            var period = new WaitForEndOfFrame();
            while (Vector3.Distance(_entity.position, to) > _stoppedDistance)
            {
                _entity.position = Vector3.Lerp(_entity.position, to, _speed * Time.deltaTime);
                yield return period;
            }
            onCompeted?.Invoke();
        }  
        
        public IEnumerator Move(Vector2 to, Action onCompeted = null)
        {
            var period = new WaitForEndOfFrame();
            Vector3 startPosition = _entity.position;
            Vector3 endPosition = to;
            float distance = Vector3.Distance(startPosition, endPosition);
            float duration = distance / _speed;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                _entity.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return period;
            }

            _entity.position = endPosition;
            onCompeted?.Invoke();
        }
        
        public IEnumerator Move(Vector2[] to, Action onCompeted = null)
        {
            var period = new WaitForEndOfFrame();
      
            for (int i = 0; i < to.Length; i++)
            {
                Vector3 startPosition = _entity.position;
                Vector3 endPosition = to[i];
                float distance = Vector3.Distance(startPosition, endPosition);
                float duration = distance / _speed;
                float elapsedTime = 0;

                while (elapsedTime < duration)
                {
                    _entity.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return period;
                }

                _entity.position = endPosition;
            }
            onCompeted?.Invoke();
        }

        public void SetEntityPosition(Vector2 pos)
        {
            _entity.position = pos;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}