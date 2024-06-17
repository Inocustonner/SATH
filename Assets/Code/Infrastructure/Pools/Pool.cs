using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Infrastructure.Pools
{
    [Serializable]
    public class MonoPool<T> where T : MonoBehaviour, IPoolEntity
    {
        [SerializeField] private Transform _root;
        [SerializeField] private T _prefab;
        [SerializeField] private List<T> _all = new();
        [SerializeField] private List<T> _enabled = new();

        public T GetNext(params object[] initParams)
        {
            T entity = GetDisabledEntity() ?? AddNewEntity(initParams);
            
            entity.EnableEntity();
            entity.gameObject.SetActive(true);
            
            _enabled.Add(entity);

            return entity;
        }

        private T GetDisabledEntity()
        {
            return _all.FirstOrDefault(entity => entity != null && !entity.gameObject.activeSelf);
        }

        private T AddNewEntity(params object[] initParams)
        {
            var newEntity = GameObject.Instantiate(_prefab, _root);
            _all.Add(newEntity);
            newEntity.InitEntity(initParams);
            return newEntity;
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
            if (entity == null || !entity.gameObject.activeSelf) return;
            entity.DisableEntity();
            entity.gameObject.SetActive(false);
            _enabled.Remove(entity);
        }

        public void DisableAll()
        {
            foreach (var mono in _all)
            {
                if (mono != null)
                {
                    mono.DisableEntity();
                    mono.gameObject.SetActive(false);
                }
            }
            _enabled.Clear();
        }
    }
}
