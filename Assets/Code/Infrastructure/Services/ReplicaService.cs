using System;
using System.Collections;
using Code.Data.DynamicData;
using Code.Data.Interfaces;
using Code.Game.Conditions;
using Code.Game.CustomActions.Actions;
using Code.Infrastructure.DI;
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
        private TextLimiter _textLimiter;
        private GameConditionProvider _gameConditionProvider;
        private ReplicaConverter _replicaConverter;
        private CoroutineRunner _coroutineRunner;
      
        [Header("Static data")]
        private LanguageSetter _languageSetter;
        private ReplicaAction[] _actions;

        [Header("Dynamic data")] 
        private Coroutine _coroutine;
        
        public event Action<AcceleratedTextData[], AnimatedTextWaiter.Mode, Action> OnStartReplica;
        public event Action<AcceleratedTextData[], AnimatedTextWaiter.Mode> OnSwitchReplicaLanguage;
        public event Action OnStopReplicaPart;
        public event Action<ReplicaConfig> OnEndReplica;

        public void GameInit()
        {
            _languageSetter = Container.Instance.FindService<LanguageSetter>();
            _inputService = Container.Instance.FindService<InputService>();
            _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();
            
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _textLimiter = Container.Instance.FindService<TextLimiter>();
            
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
                _languageSetter.OnSwitchLanguage += OnSwitchLanguage;
                _inputService.OnPressInteractionKey += OnPressInteractionKey;
                foreach (var action in _actions)
                {
                    action.OnTryStartReplica += OnTryStartReplica;
                }
            }
            else
            {
                _languageSetter.OnSwitchLanguage -= OnSwitchLanguage;
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
                foreach (var action in _actions)
                {
                    action.OnTryStartReplica -= OnTryStartReplica;
                }
            }
        }

        private void OnSwitchLanguage()
        {
            _coroutineRunner.StopRoutine(_coroutine);
            _coroutine = _coroutineRunner.StartRoutine(WaitTextLimiterAfterSwitch());
        }

        private IEnumerator WaitTextLimiterAfterSwitch()
        {
            yield return new WaitUntil(() => _textLimiter.IsUnlock);
            if (_replicaConverter.TryGetCurrentConfig(out var config) &&
                _replicaConverter.TryGetAcceleratedTexts(_languageSetter.Language, out var replica))
            {
                var waitedMode = config.IsBlockMovement
                ? AnimatedTextWaiter.Mode.PressKey
                : AnimatedTextWaiter.Mode.Time;

                OnSwitchReplicaLanguage?.Invoke(replica, waitedMode);
            }
        }

        private void OnTryStartReplica(ReplicaConfig replicaConfig)
        {
            _replicaConverter.SetConfig(replicaConfig);
            if (_replicaConverter.TryGetAcceleratedTexts(_languageSetter.Language, out var replicas))
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