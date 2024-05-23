using System;
using System.Collections.Generic;
using Code.CustomActions.Actions;
using Code.Data.Configs;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class ReplicaService : IService, IGameInitListener, IGameStartListener, IGameExitListener
    {
        [Header("Services")]
        private InputService _inputService;
        private MoveLimiter _moveLimiter;
        [Header("Static data")]
        private GameSettings _gameSettings;
        private List<ReplicaAction> _actions;

        public event Action<string, float, Action> OnStartReplica;
        public event Action OnStopReplica;

        public void GameInit()
        {
            _gameSettings = Container.Instance.FindService<GameSettings>();
            _inputService = Container.Instance.FindService<InputService>();
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _actions = Container.Instance.GetContainerComponents<ReplicaAction>();
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
            if (replicaConfig.TryGetLocalizedReplica(_gameSettings.Language, out var replica))
            {
                this.Log($"invoke start {replicaConfig.name}");

                if (replicaConfig.IsBlockMovement)
                {
                    _moveLimiter.Block();
                }
                
                OnStartReplica?.Invoke(replica, replicaConfig.WriteSpeed, () =>
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
            OnStopReplica?.Invoke();
        }
    }
}