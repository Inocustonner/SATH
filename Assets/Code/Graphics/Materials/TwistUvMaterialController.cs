using System;
using System.Collections;
using Code.Data.Enums;
using Code.Data.Values.RangeFloat;
using UnityEngine;
using CoroutineRunner = Code.Infrastructure.Services.CoroutineRunner;

namespace Code.Materials
{
    [Serializable]
    public class TwistUvMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._TwistUvAmount;
        [MinMaxRangeFloat(0, 3.14f)] public RangedFloat Range;
        [SerializeField] private float _speed = 0.15f;

        [SerializeField] private bool _isPlaying;
        private bool _isMoveUp;
        private Coroutine _coroutine;
        private CoroutineRunner _coroutineRunner;

        public void Init(CoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void StartPlay()
        {
            
            SetFloatValue(Range.MinValue);
            _isPlaying = true;
            _coroutine = _coroutineRunner.StartRoutine(Play());
        }

        public void StopPlay()
        {
            _isPlaying = false;
            _coroutineRunner.StopRoutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator Play()
        {
            var period = new WaitForEndOfFrame();
            while (_isPlaying)
            {
                float newValue;
                if (_isMoveUp)
                {
                     newValue = GetFloatValue() + _speed * Time.deltaTime;
                     if (newValue >= Range.MaxValue)
                     {
                         _isMoveUp = false;
                     }
                }
                else
                {
                    newValue = GetFloatValue() - _speed * Time.deltaTime;
                    if (newValue <= Range.MinValue)
                    {
                        _isMoveUp = true;
                    }
                }
                SetFloatValue(newValue);
                yield return period;
            }
        }
    }
}