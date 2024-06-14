using System;
using System.Linq;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Game.Conditions;
using Code.Game.CustomActions.Actions;
using Code.Game.Parts;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class TransitionGamePartService : IService, IGameInitListener, IGameStartListener, IGameExitListener
    {
        [Header("Services")] 
        private GameConditionProvider _gameConditionProvider;
        private CoroutineRunner _coroutineRunner;
        
        [Header("Components")] 
        private GamePart[] _gameParts;
        private TransitionNextGamePartAction[] _actions;

        [Header("Static data")] 
        private float _delay;
        
        [Header("Dinamyc data")] 
        private GamePart _currentPart;
        private Coroutine _transitionCoroutine;
        
        public event Action OnStartTransition;

        public void GameInit()
        {
            _gameConditionProvider = Container.Instance.FindService<GameConditionProvider>();
            _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();

            _gameParts = Container.Instance.GetContainerComponents<GamePart>();
            _actions = Container.Instance.GetContainerComponents<TransitionNextGamePartAction>();

            _delay = Container.Instance.FindConfig<UIConfig>().CurtainDuration / 2;

            _currentPart = _gameParts.FirstOrDefault(p => p.gameObject.activeInHierarchy);
        }
        
        public void GameStart()
        {
            SubscribeToEvents(true);
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                foreach (var transitionAction in _actions)
                {
                    transitionAction.OnTryStart += OnTryStartGamePartTransition;
                }

                foreach (var gamePart in _gameParts)
                {
                    gamePart.OnTryRestart += OnTryRestartGamePart;
                }
            }
            else
            {
                foreach (var transitionAction in _actions)
                {
                    transitionAction.OnTryStart -= OnTryStartGamePartTransition;
                }
            }
        }

        private void OnTryRestartGamePart(GamePart part)
        {
            OnStartTransition?.Invoke();
            _coroutineRunner.StopRoutine(_transitionCoroutine);
            _transitionCoroutine = _coroutineRunner.StartActionWithDelay(part.Restart, _delay);
        }

        private void OnTryStartGamePartTransition(NextGamePartData[] nextPartsData)
        {
            foreach (var nextPart in nextPartsData)
            {
                if (_gameConditionProvider.GetValue(nextPart.Condition))
                {
                    var gamePart = _gameParts.FirstOrDefault(p => p.GamePartName == nextPart.GamePartName);
                    OnStartTransition?.Invoke();
                    _coroutineRunner.StartActionWithDelay(() =>
                    {
                        _currentPart.gameObject.SetActive(false);
                        _currentPart = gamePart;
                        _currentPart.gameObject.SetActive(true);
                    }, _delay);
                }
            }
        }
    }
}