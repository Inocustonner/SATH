using Code.Data.Configs;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.UI
{
    public class AnimatedTextWaiter: IService, IGameInitListener
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
            _isReady = true;
        }
    }
}