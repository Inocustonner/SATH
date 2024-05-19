using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Save;
using UnityEngine;

namespace Code.Infrastructure.DI
{
    public class Container : MonoBehaviour
    {
        public static Container Instance;
        [SerializeField] private List<ScriptableObject> _configs;

        private MonoBehaviour[] _allObjects;
        private List<IService> _services = new();
        private List<IMono> _mono = new();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            _allObjects = FindObjectsOfType<MonoBehaviour>();
            InitList(ref _services);
            InitList(ref _mono);
        }

        private void InitList<T>(ref List<T> list)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            var serviceTypes = types.Where(t =>
                typeof(T).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !typeof(MonoBehaviour).IsAssignableFrom(t));

            foreach (var serviceType in serviceTypes)
            {
                if (Activator.CreateInstance(serviceType) is T service)
                {
                    list.Add(service);
                }
            }

            var mbServices = _allObjects.OfType<T>();
            if (mbServices.Any())
            {
                list.AddRange(mbServices);
            }

     
        }

     
        public T FindConfig<T>() where T : ScriptableObject
        {
            foreach (var scriptableObject in _configs)
            {
                if (scriptableObject is T findConfig)
                {
                    return findConfig;
                }
            }

            return null;
        }

        public T FindService<T>() where T : IService
        {
            foreach (var service in _services)
            {
                if (service is T findService)
                {
                    return findService;
                }
            }

            return default;
        }

      

        public List<IGameListeners> GetGameListeners()
        {
            return GetContainerComponents<IGameListeners>();
        }

        public List<IProgressReader> GetProgressReaders()
        {
            return GetContainerComponents<IProgressReader>();
        }

        private List<T> GetContainerComponents<T>()
        {
            var list = new List<T>();

            list.AddRange(_services.OfType<T>().ToList());
            list.AddRange(_mono.OfType<T>().ToList());

            var mbListeners = _allObjects.OfType<T>();
            foreach (var mbListener in mbListeners)
            {
                if (!list.Contains(mbListener))
                {
                    list.Add(mbListener);
                }
            }

            return list;
        }
    }
}