using Code.Data.Interfaces;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Audio.Params
{
    public class AudioVelocityParam: MonoBehaviour, IGameStartListener, IPartTickListener, IPartExitListener
    {
        [SerializeField] private AudioParameterController _audioParameter;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _lerpSpeed = 1;
        
        [Header("Debug")]
        [SerializeField] private float _currentValue;
        [SerializeField] private float _magnitude;
        public void GameStart()
        {
            this.Log("MY START",Color.magenta);
            _audioParameter.InitParam();
        }
        
        public void PartTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _currentValue = _audioParameter.GetValue();
            _magnitude = Mathf.Clamp(_rigidbody.velocity.magnitude,0,1);
            
            if (_currentValue != _magnitude)
            {
                var value = Mathf.Lerp(_currentValue,_magnitude, _lerpSpeed * Time.deltaTime);
                _audioParameter.SetValue(value);
            }
        }

        public void PartExit()
        {
            _audioParameter.SetValue(0);
        }
    }
}