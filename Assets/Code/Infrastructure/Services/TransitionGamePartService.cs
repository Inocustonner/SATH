using System;
using System.Collections;
using System.Linq;
using Code.CustomActions.Actions;
using Code.Data.Configs;
using Code.Data.Enums;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Replicas;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class TransitionGamePartService : IService, IGameInitListener, IGameStartListener, IGameExitListener
    {
        [Header("Services")] 
        private GameConditionService _gameConditionService;
        private CoroutineRunner _coroutineRunner;
        
        [Header("Components")] 
        private GamePart[] _gameParts;
        private TransitionNextGamePartAction[] _actions;
        
        [Header("Static data")] 
        private float _delay;
        
        [Header("Dinamyc data")] 
        private GamePart _currentPart;
        
        public event Action OnStartTransition;

        public void GameInit()
        {
            _gameConditionService = Container.Instance.FindService<GameConditionService>();
            _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();

            _gameParts = Container.Instance.GetContainerComponents<GamePart>();
            _actions = Container.Instance.GetContainerComponents<TransitionNextGamePartAction>();

            _delay = Container.Instance.FindConfig<UIConfig>().CurtainDuration / 2;

            _currentPart = _gameParts.FirstOrDefault(p => p.GamePartName == GamePartName.Part_1__home);
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
                    transitionAction.OnTryStart += OnTryStartTransition;
                }
            }
            else
            {
                foreach (var transitionAction in _actions)
                {
                    transitionAction.OnTryStart -= OnTryStartTransition;
                }
            }
        }

        private void OnTryStartTransition(NextGamePartData[] nextPartsData)
        {
            foreach (var nextPart in nextPartsData)
            {
                if (_gameConditionService.GetValue(nextPart.Condition))
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