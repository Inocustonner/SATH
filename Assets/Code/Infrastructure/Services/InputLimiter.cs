using System;
using Code.Infrastructure.DI;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public abstract class Limiter: IService
    {
        private int _counter;
        public bool IsUnlock => _counter == 0;
        public event Action<bool> OnSwitch; 
        
        public void Block()
        {
            _counter++;
            
            if (_counter == 1)
            {
                OnSwitch?.Invoke(false);
            }
            
            this.Log($"block {_counter}",Color.gray);
        }

        public void Unblock()
        {
            if (_counter > 0)
            {
                _counter--;
                
                if (_counter == 0)
                {
                    OnSwitch?.Invoke(true);
                }
                this.Log($"unblock {_counter}",Color.gray);
            }
            else
            {
                this.Log($"can`t unblock {_counter}",Color.black);
            }
        }
    }

    public class MoveLimiter : Limiter
    {
        
    }

    public class InteractionLimiter : Limiter
    {
        
    }
}