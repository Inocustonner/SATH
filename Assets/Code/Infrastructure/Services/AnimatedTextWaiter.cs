using Code.Data.Configs;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class AnimatedTextWaiter : IService, IGameInitListener, IGameTickListener
    {
        public enum Mode
        {
            Time,
            PressKey
        }

        public bool IsReadyDelay => _isReadyDelay;
        public bool IsReadySkip => _currentSkipDelay >= _skipDelay;

        [Header("Services")] 
        private InputService _inputService;
        private CoroutineRunner _coroutineRunner;

        [Header("Static data")] 
        private float _waitTime;
        private float _skipDelay;

        [Header("Dinamyc data")] 
        private Mode _currentMode;
        private bool _isReadyDelay;
        private float _currentSkipDelay;


        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
            _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();

            var uiConfig = Container.Instance.FindConfig<UIConfig>();
            _waitTime = uiConfig.ReplicaDelaySeconds;
            _skipDelay = uiConfig.ReplicaSkipDelaySeconds;
        }

        public void GameTick()
        {
            if (_currentSkipDelay < _skipDelay)
            {
                _currentSkipDelay += Time.deltaTime;
            }

        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _inputService.OnPressInteractionKey += OnPressInteractionKey;
            }
            else
            {
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
            }
        }

        public void ResetSkipDelay()
        {
            _currentSkipDelay = 0;
        }

        public void SetMode(Mode mode)
        {
            _currentMode = mode;
        }

        public void StartWait()
        {
            switch (_currentMode)
            {
                case Mode.Time:
                    _coroutineRunner.StartActionWithDelay(() => _isReadyDelay = true, _waitTime);
                    break;
                case Mode.PressKey:
                    SubscribeToEvents(true);
                    break;
            }
        }

        public void Reset()
        {
            _isReadyDelay = false;
            if (_currentMode == Mode.PressKey)
            {
                SubscribeToEvents(false);
            }
        }

        private void OnPressInteractionKey()
        {
            if (_currentSkipDelay >= _skipDelay)
            {
                _isReadyDelay = true;
            }
        }
    }
}