using System;
using Code.Infrastructure.DI;

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