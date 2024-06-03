using Code.Data.Configs;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class AnimatedTextWaiter: IService, IGameInitListener, IGameTickListener
    {
        public enum Mode
        {
            Time,
            PressKey
        }

        [Header("Services")]
        private InputService _inputService;
        private CoroutineRunner _coroutineRunner;
        [Header("Static data")]
        private float _waitTime;
        [Header("Dinamyc data")]
        private Mode _currentMode;
        private bool _isReady;
        private float _skipCooldown;

        public bool IsReady => _isReady;
        
        
        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
            _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();
            _waitTime = Container.Instance.FindConfig<UIConfig>().ReplicaDelaySeconds;
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
            _skipCooldown = 0;
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
                    _coroutineRunner.StartActionWithDelay(() => _isReady = true, _waitTime);
                    break;
                case Mode.PressKey:
                    SubscribeToEvents(true);
                    break;
            }
        }

        public void Reset()
        {
            _isReady = false;
            if (_currentMode == Mode.PressKey)
            {
                SubscribeToEvents(false);
            }
        }

        private void OnPressInteractionKey()
        {
            if (_skipCooldown >= 0.75f)
            {
                _isReady = true;
            }
        }

        public void GameTick()
        {
            if (_skipCooldown <= 0.75f)
            {
                _skipCooldown += Time.deltaTime;
            }
        }
    }
}