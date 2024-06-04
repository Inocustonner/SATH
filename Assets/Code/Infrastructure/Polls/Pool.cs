using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Infrastructure.Polls
{
    [Serializable]
    public class MonoPool<T> where T: MonoBehaviour, IPoolEntity
    {
        [SerializeField] private Transform _root;
        [SerializeField] private T _prefab;
        [SerializeField] private List<T> _all;
        [SerializeField] private List<T> _enabled;

        private int _nextIndex;

        public T GetNext(params object[] initParams)
        {
            var next = Get(_nextIndex, initParams);
            _nextIndex++;
            return next;
        }

        public T Get(int index, params object[] initParams)
        {
            T entity = null;
            
            if (index >= _all.Count)
            {
                entity = AddNewEntity();
                entity.InitEntity(initParams);
            }
        
            entity ??= _all[index];
            entity.EnableEntity();
            
            _enabled.Add(entity);
            
            return entity;
        }

        private T AddNewEntity()
        {
            var newButton = GameObject.Instantiate(_prefab,_root);
            _all.Add(newButton);
            return newButton;
        }

        public IEnumerable<T> GetAll()
        {
            return _all;
        }

        public IEnumerable<T> GetAllEnabled()
        {
            return _enabled;
        }

        public void Disable(T entity)
        {
            entity.gameObject.SetActive(false);
            _enabled.Remove(entity);
        }

        public void DisableAll()
        {
            _nextIndex = 0;
            foreach (var mono in _all)
            {
                mono.DisableEntity();
                mono.gameObject.SetActive(false);
            }
            _enabled.Clear();
        }
    }
}