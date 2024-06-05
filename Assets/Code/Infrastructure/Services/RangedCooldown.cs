using System;
using System.Collections;
using Code.Data.Values.RangeFloat;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    [Serializable]
    public class RangedCooldown: Cooldown
    {
        [SerializeField,MinMaxRangeFloat(0,100)] private RangedFloat _rangedFloat;
        
        
        public RangedCooldown(RangedFloat rangedFloat)
        {
            _rangedFloat = rangedFloat;
        }
        
        protected override IEnumerator LoopChecking(Action onCompleted)
        {
            while (_isLoop)
            {
                yield return new WaitForSeconds(_rangedFloat.GetRandomValue());
                onCompleted?.Invoke();
            }
        }

        protected override IEnumerator Checking(Action onCompleted)
        {
            yield return new WaitForSeconds(_rangedFloat.GetRandomValue());
            onCompleted?.Invoke();
        }
    }
}