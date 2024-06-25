using System;

namespace Code.Game.Components.Destruction
{
    [Serializable]
    public class Health 
    {
        public int Current { get; private set; }
        public int Max { get; private set;}

        public event Action OnChanged;
        public event Action OnDeath;

        public Health(int max)
        {
            Max = max;
        }

        public void Reset()
        {
            Current = Max;
            OnChanged?.Invoke();    
        }

        public void GetDamage(int damage)
        {
            Current -= damage;
            if (Current < 0)
            {
                Current = 0;
                OnDeath?.Invoke();
                return;
            }
            OnChanged?.Invoke();    
        }
    }
}