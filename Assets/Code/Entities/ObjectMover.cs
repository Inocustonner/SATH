using System;
using System.Collections;
using UnityEngine;

namespace Code.Entities
{
    [Serializable]
    public class ObjectMover
    {
        [SerializeField] private Transform _entity;
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _stoppedDistance = 0.1f;
        
        public Transform Entity => _entity;
        
        public IEnumerator Move(Vector2 to, Action onCompeted = null)
        {
            var period = new WaitForEndOfFrame();
            while (Vector3.Distance(_entity.position, to) > _stoppedDistance)
            {
                _entity.position = Vector3.Lerp(_entity.position, to, _speed * Time.deltaTime);
                yield return period;
            }
            onCompeted?.Invoke();
        }
        
        public IEnumerator Move(Vector2[] to, Action onCompeted = null)
        {
            var period = new WaitForEndOfFrame();
            for (int i = 0; i < to.Length; i++)
            {
                while (Vector3.Distance(_entity.position, to[i]) > _stoppedDistance)
                {
                    _entity.position = Vector3.Lerp(_entity.position, to[i], _speed * Time.deltaTime);
                    yield return period;
                }
            }
            onCompeted?.Invoke();
        }

        public void Follow(Transform to, Vector2 offset = new())
        {
            
        }


 
    }
}