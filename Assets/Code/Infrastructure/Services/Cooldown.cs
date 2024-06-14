using System;
using System.Collections;
using Code.Infrastructure.DI;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] protected float _maxTime;
        protected bool _isLoop;
        
        private CoroutineRunner _coroutineRunner;
        protected Coroutine _coroutine;
        
        public void Start(bool isLoop, Action onCompleted)
        {
            if (_coroutineRunner == null)
            {
                _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();
            }

            _isLoop = isLoop;
            _coroutine = _coroutineRunner.StartCoroutine(_isLoop ? LoopChecking(onCompleted) : Checking(onCompleted));
        }

        public void Stop()
        {
            _isLoop = false;
            if (_coroutineRunner.IsDestroyed())
            {
                return;
            }
            _coroutineRunner.StopRoutine(_coroutine);
        }

        protected virtual IEnumerator LoopChecking(Action onCompleted)
        {
            var period = new WaitForSeconds(_maxTime);
            while (_isLoop)
            {
                yield return period;
                onCompleted?.Invoke();
            }
        }
        
        protected virtual IEnumerator Checking(Action onCompleted)
        {
            yield return new WaitForSeconds(_maxTime);
            onCompleted?.Invoke();
        }
    }
}