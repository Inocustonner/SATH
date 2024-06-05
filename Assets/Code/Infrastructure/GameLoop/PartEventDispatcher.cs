using System.Collections.Generic;
using System.Linq;
using Code.Data.Interfaces;
using Code.GameParts;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Infrastructure.GameLoop
{
    public class PartEventDispatcher : MonoBehaviour,
        IGameInitListener,
        IGameStartListener,
        IGameTickListener,
        IGameFixedTickListener,
        IGameExitListener
    {
        [SerializeField] private GamePart _gamePart;
        [SerializeField] private GameObject[] _childObjects;
        private readonly Dictionary<GameObject, bool> _childStartStates = new();

        private IPartStartListener[] _partStartListeners;
        private IPartTickListener[] _partTickListeners;
        private IPartFixedTickListener[] _partFixedTickListeners;
        private IPartExitListener[] _partExitListeners;
        private IRestarable[] _restartable;

        private GameEventDispatcher _gameEventDispatcher;

        public void GameInit()
        {
            _gameEventDispatcher = Container.Instance.FindService<GameEventDispatcher>();
            _gameEventDispatcher.RemoveUpdateListener(this);
            _childObjects = GetComponentsInChildren<Transform>(true).Select(child => child.gameObject).ToArray();

            foreach (var childObject in _childObjects)
            {
                _childStartStates.Add(childObject, childObject.activeSelf);
            }

            _restartable = GetComponentsInChildren<IRestarable>().ToArray();
            _partStartListeners = GetComponentsInChildren<IPartStartListener>().ToArray();
            _partTickListeners = GetComponentsInChildren<IPartTickListener>().ToArray();
            _partFixedTickListeners = GetComponentsInChildren<IPartFixedTickListener>().ToArray();
            _partExitListeners = GetComponentsInChildren<IPartExitListener>().ToArray();
        }

        public void GameStart()
        {
            SubscribeToEvents(true);
        }

        private void OnEnable()
        {
            foreach (var listener in _partStartListeners)
            {
                listener.PartStart();
            }
            
            _gameEventDispatcher.AddUpdateListener(this);
        }

        public void GameTick()
        {
            foreach (var listener in _partTickListeners)
            {
                listener.PartTick();
            }
        }

        public void GameFixedTick()
        {
            foreach (var listener in _partFixedTickListeners)
            {
                listener.PartFixedTick();
            }
        }

        private void OnGamePartRestart()
        {
            foreach (var restartable in _restartable)
            {
                restartable.Restart();
            }
        }

        private void OnDisable()
        {
            _gameEventDispatcher.RemoveUpdateListener(this);
            
            foreach (var listener in _partExitListeners)
            {
                listener.PartExit();
            }
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _gamePart.OnRestart += OnGamePartRestart;
            }
            else
            {
                _gamePart.OnRestart -= OnGamePartRestart;
            }
        }

        private void OnValidate()
        {
            if (_gamePart == null)
            {
                TryGetComponent(out _gamePart);
            }
        }
    }
}