using System;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Factory
{
    public class PushListener: MonoBehaviour, IPartTickListener
    {
        [SerializeField] private Rigidbody2D _stone;
        [SerializeField] private float _pushTime;
        [SerializeField] private float _lastPushAttemptTime;

        public bool IsPushed { get; private set; }
        public event Action OnStartPush;
        public event Action OnStopPush;
        
        public void PartTick()
        {
            if (_stone.velocity.magnitude > 0)
            {
                _pushTime += Time.deltaTime;
                _lastPushAttemptTime += Time.deltaTime;
                if (!IsPushed)
                {
                    OnStartPush?.Invoke();
                    IsPushed = true;
                }
            }
            else if(_lastPushAttemptTime != 0)
            {
                _lastPushAttemptTime = 0;
            }
            else if(IsPushed)
            {
                OnStopPush?.Invoke();
                IsPushed = false;
            }
        }
        
        public float GetPushTime()
        {
            return _pushTime;
        }
    }
}