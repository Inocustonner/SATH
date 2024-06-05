using System.Collections.Generic;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Infrastructure.GameLoop
{
    public class GameEventDispatcher : MonoBehaviour, IService
    {
        private readonly List<IGameInitListener> _initListeners = new();
        private readonly List<IGameLoadListener> _loadListeners = new();
        private readonly List<IGameStartListener> _startListeners = new();
        private readonly List<IGameTickListener> _tickListeners = new();
        private readonly List<IGameFixedTickListener> _fixedTickListeners = new();
        private readonly List<IGameExitListener> _exitListeners = new();

        private void Awake()
        {
            InitializeListeners();
            NotifyGameInit();
            NotifyGameLoad();
        }

        private void Start()
        {
            NotifyGameStart();
        }

        private void Update()
        {
            NotifyGameTick();
        }

        private void FixedUpdate()
        {
            NotifyGameFixedTick();
        }

        private void OnApplicationQuit()
        {
            NotifyGameExit();
        }

        public void AddUpdateListener(IGameListener listener)
        {
            if (listener is IGameTickListener tickListener && !_tickListeners.Contains(tickListener))
                _tickListeners.Add(tickListener);
            if (listener is IGameFixedTickListener fixedTickListener && !_fixedTickListeners.Contains(fixedTickListener))
                _fixedTickListeners.Add(fixedTickListener);
        }

        public void RemoveUpdateListener(IGameListener listener)
        {
            if (listener is IGameTickListener tickListener && _tickListeners.Contains(tickListener))
                _tickListeners.Remove(tickListener);
            if (listener is IGameFixedTickListener fixedTickListener && _fixedTickListeners.Contains(fixedTickListener))
                _fixedTickListeners.Remove(fixedTickListener);
        }
        
        private void InitializeListeners()
        {
            var gameListeners = Container.Instance.GetContainerComponents<IGameListener>();

            foreach (var listener in gameListeners)
            {
                AddListener(listener);
            }
        }
        private void AddListener(IGameListener listener)
        {
            if (listener is IGameInitListener initListener)
                _initListeners.Add(initListener);
            if (listener is IGameLoadListener loadListener)
                _loadListeners.Add(loadListener);
            if (listener is IGameStartListener startListener)
                _startListeners.Add(startListener);
            if (listener is IGameTickListener tickListener)
                _tickListeners.Add(tickListener);
            if (listener is IGameFixedTickListener fixedTickListener)
                _fixedTickListeners.Add(fixedTickListener);
            if (listener is IGameExitListener exitListener)
                _exitListeners.Add(exitListener);
        }

        private void NotifyGameInit()
        {
            foreach (var listener in _initListeners)
            {
                listener.GameInit();
            }
        }

        private void NotifyGameLoad()
        {
            foreach (var listener in _loadListeners)
            {
                listener.GameLoad();
            }
        }

        private void NotifyGameStart()
        {
            foreach (var listener in _startListeners)
            {
                listener.GameStart();
            }
        }

        private void NotifyGameTick()
        {
            foreach (var listener in _tickListeners)
            {
                listener.GameTick();
            }
        }

        private void NotifyGameFixedTick()
        {
            foreach (var listener in _fixedTickListeners)
            {
                listener.GameFixedTick();
            }
        }
        
        private void NotifyGameExit()
        {
            foreach (var listener in _exitListeners)
            {
                listener.GameExit();
            }
        }
    }
}