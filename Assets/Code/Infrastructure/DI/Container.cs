using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.DI
{
    public class Container : MonoBehaviour
    {
        public static Container Instance;
        [Header("Initialized by reference")] 
        [SerializeField] private List<ScriptableObject> _configs;

        [Header("Initializes automatically")] 
        [SerializeField] private MonoBehaviour[] _allObjects;
        private List<IEntity> _entities;
        private List<IService> _services;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;

            _allObjects = FindAllObjectsOfType<MonoBehaviour>().ToArray();
            InitList(ref _entities);
            _services = _entities.OfType<IService>().ToList();
        }

        private void InitList<T>(ref List<T> list)
        {
            list = new List<T>();
            var types = Assembly.GetExecutingAssembly().GetTypes();

            var serviceTypes = types.Where(t =>
                typeof(T).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract &&
                !typeof(MonoBehaviour).IsAssignableFrom(t));

            foreach (var serviceType in serviceTypes)
            {
                if (Activator.CreateInstance(serviceType) is T service)
                {
                    list.Add(service);
                }
            }

            var mbServices = _allObjects.OfType<T>();
            list.AddRange(mbServices);
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

        public T[] GetContainerComponents<T>()
        {
            var components = _entities.OfType<T>().ToList();
            var objectComponents = _allObjects.OfType<T>();
            foreach (var mbListener in objectComponents)
            {
                if (!components.Contains(mbListener))
                {
                    components.Add(mbListener);
                }
            }

            return components.ToArray();
        }

        private List<T> FindAllObjectsOfType<T>() where T : UnityEngine.Object
        {
            var sceneObjects = new List<T>();
            Scene currentScene = SceneManager.GetActiveScene();

            foreach (var rootGameObject in currentScene.GetRootGameObjects())
            {
                T[] objectsInRoot = rootGameObject.GetComponentsInChildren<T>(true);
                sceneObjects.AddRange(objectsInRoot);
            }


            var results = sceneObjects.Where(obj =>
                obj is T && !obj.GetType().IsAbstract && obj.GetType().Namespace?.StartsWith("Code") == true).ToList();
            return results.Distinct().ToList();
        }
    }
}