using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Save;
using Code.UI.Base;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.DI
{
    public class Container : MonoBehaviour
    {
        public static Container Instance;
        [SerializeField] private List<ScriptableObject> _configs;

        [SerializeReference] private MonoBehaviour[] _allObjects;
        [SerializeReference] private List<IService> _services = new();
        [SerializeReference] private List<IMono> _mono = new();
        [SerializeReference] private List<IMenuPresenter> _presenters = new();
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            _allObjects = FindAllObjectsOfType<MonoBehaviour>().ToArray();
            InitList(ref _services);
            InitList(ref _mono);
            InitList(ref _presenters);
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
        
        public T FindUIPresenter<T>() where T : IService
        {
            foreach (var presenter in _presenters)
            {
                if (presenter is T findUIPresenter)
                {
                    return findUIPresenter;
                }
            }
            return default;
        }
        
        public List<T> GetContainerComponents<T>()
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
        
        private List<T> FindAllObjectsOfType<T>() where T : UnityEngine.Object
        {
            List<T> results = new List<T>();
            Type type = typeof(T);
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;

            MethodInfo method = typeof(UnityEngine.Object).GetMethod("FindObjectsOfTypeAll", bindingFlags);

            if (method == null)
            {
                Debug.LogError("Method 'FindObjectsOfTypeAll' not found.");
                return results;
            }

            var objects = (UnityEngine.Object[])method.Invoke(null, new object[] { type });
            results.AddRange(objects.OfType<T>());

            foreach (var o in objects)
            {
                this.Log($"{typeof(T).Name} {o.name}");
            }

            var uniqueList = new List<T>();

            foreach (var result in results)
            {
                if (uniqueList.Any(r => r.GetHashCode() == result.GetHashCode()))
                {
                    continue;
                }
                else
                {
                    uniqueList.Add(result);
                }
            }
            return uniqueList;
        }
    }
}