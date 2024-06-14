using Code.Data.Interfaces;
using Code.Infrastructure.Audio.AudioEvents;
using Code.Infrastructure.Audio.Params;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Game.Components.Factory
{
    public class RightPusherAudioEvent : MonoBehaviour,IGameInitListener ,IPartStartListener,IPartTickListener ,IPartExitListener
    {
        [Header("Components")]
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private AudioEvent _audioEvent;
        [SerializeField] private AudioParameterController _audioParameter;

        [Header("Param")] 
        [SerializeField] private float _paramChangeSpeed = 1;

        [Header("Service")] 
        private InputService _inputService;
        
        private float _paramValue;
        private bool _isTouched;
        private bool _isPlay;

        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
            _audioParameter.InitParam();
        }

        public void PartStart()
        {
            SubscribeToEvents(true);
        }

        public void PartTick()
        {
            if (_isTouched)
            {
                var paramValue = Mathf.Clamp(_inputService.GetDirection().x, 0, 1);
                    _audioParameter.SetValue(
                        Mathf.Lerp(_audioParameter.GetValue(),
                        paramValue,
                        _paramChangeSpeed * Time.deltaTime));
                if (_isPlay)
                {
                  
                    if (paramValue == 0)
                    {
                        _isPlay = false;
                    }
                }
                else if(paramValue > 0)
                {
                    _isPlay = true;
                    _audioEvent.PlayAudioEvent();
                }
            }
            else if(_audioParameter.GetValue() > 0)
            {
                _audioParameter.SetValue(Mathf.Lerp(_audioParameter.GetValue(), 0, _paramChangeSpeed * Time.deltaTime));
            }
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _collisionObserver.OnEnter += OnEnterCollision;
                _collisionObserver.OnExit += OnExitCollision;
            }
            else
            {
                _collisionObserver.OnEnter -= OnEnterCollision;
                _collisionObserver.OnExit -= OnExitCollision;
            }
        }

        private void OnEnterCollision(GameObject obj)
        {
            _isTouched = true;
        }

        private void OnExitCollision(GameObject obj)
        {
            _isTouched = false;
            _isPlay = false;
        }
    }
}
