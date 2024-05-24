using System;
using System.Collections.Generic;
using Code.CustomActions.Actions;
using Code.Data.DynamicData;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Scenarios.Scripts;
using Code.UI;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class ReplicaService : IService, IGameInitListener, IGameStartListener, IGameExitListener
    {
        [Header("Services")]
        private InputService _inputService;
        private MoveLimiter _moveLimiter;
        private GameConditionService _gameConditionService;
        private ReplicaConverter _replicaConverter;
        [Header("Static data")]
        private GameSettings _gameSettings;
        private List<ReplicaAction> _actions;
        public event Action<AcceleratedText[], AnimatedTextWaiter.Mode, Action> OnStartReplica;
        public event Action OnStopReplicaPart;

        public void GameInit()
        {
            _gameSettings = Container.Instance.FindService<GameSettings>();
            _inputService = Container.Instance.FindService<InputService>();
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _actions = Container.Instance.GetContainerComponents<ReplicaAction>();
            _replicaConverter = new ReplicaConverter(Container.Instance.FindService<GameConditionService>());
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
                _inputService.OnPressInteractionKey += OnPressInteractionKey;
                this.Log($"subscribe {_actions.Count}");
                foreach (var action in _actions)
                {
                    action.OnTryStartReplica += OnTryStartReplica;
                }
            }
            else
            {
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
                foreach (var action in _actions)
                {
                    action.OnTryStartReplica -= OnTryStartReplica;
                }
            }
        }

        private void OnTryStartReplica(ReplicaConfig replicaConfig)
        {
            this.Log($"try start {replicaConfig.name}");
            //todo восстановить функционал
            
            _replicaConverter.SetConfig(replicaConfig);
            if (_replicaConverter.TryGetAcceleratedTexts(_gameSettings.Language, out var replicas))
            {
                this.Log($"invoke start {replicaConfig.name}");

                if (replicaConfig.IsBlockMovement)
                {
                    _moveLimiter.Block();
                }
                
                var waitedMode = replicaConfig.IsBlockMovement
                    ? AnimatedTextWaiter.Mode.PressKey
                    : AnimatedTextWaiter.Mode.Time;
                
                OnStartReplica?.Invoke(replicas, waitedMode, () =>
                {
                    if (replicaConfig.IsBlockMovement)
                    {
                        _moveLimiter.Unblock();
                    }
                });
            }
        }

        private void OnPressInteractionKey()
        {
            OnStopReplicaPart?.Invoke();
        }
    }
}