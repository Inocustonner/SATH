using System;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class Health : MonoBehaviour
    {
        public int Current { get; private set; }
        public int Max { get; private set;}

        public event Action OnChanged;
        public event Action OnDeath;
        
        public void Set(int max)
        {
            Max = max;
        }

        public void Reset()
        {
            this.Log($"{gameObject.name} Reset");
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