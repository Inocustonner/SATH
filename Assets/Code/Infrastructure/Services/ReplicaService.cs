using System;
using Code.Data.DynamicData;
using Code.Data.Interfaces;
using Code.GameParts.CustomActions.Actions;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services.Conditions;
using Code.Replicas.Scripts;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class ReplicaService : IService, IGameInitListener, IGameStartListener, IGameExitListener
    {
        [Header("Services")]
        private InputService _inputService;
        private MoveLimiter _moveLimiter;
        private GameConditionProvider _gameConditionProvider;
        private ReplicaConverter _replicaConverter;
      
        [Header("Static data")]
        private GameSettings _gameSettings;
        private ReplicaAction[] _actions;

        
        public event Action<AcceleratedTextData[], AnimatedTextWaiter.Mode, Action> OnStartReplica;
        public event Action OnStopReplicaPart;
        public event Action<ReplicaConfig> OnEndReplica;

        public void GameInit()
        {
            _gameSettings = Container.Instance.FindService<GameSettings>();
            _inputService = Container.Instance.FindService<InputService>();
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _actions = Container.Instance.GetContainerComponents<ReplicaAction>();
            _replicaConverter = new ReplicaConverter(Container.Instance.FindService<GameConditionProvider>());
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
            _replicaConverter.SetConfig(replicaConfig);
            if (_replicaConverter.TryGetAcceleratedTexts(_gameSettings.Language, out var replicas))
            {
                if (replicaConfig.IsBlockMovement)
                {
                    this.Log($"replica {replicaConfig.name} is block movement");
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
                    OnEndReplica?.Invoke(replicaConfig);
                });
            }
        }

        private void OnPressInteractionKey()
        {
            OnStopReplicaPart?.Invoke();
        }
    }
}