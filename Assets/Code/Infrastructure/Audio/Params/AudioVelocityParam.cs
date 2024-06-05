using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Audio.AudioEvents
{
    public class AudioVelocityParam: MonoBehaviour, IGameStartListener, IGameTickListener
    {
        [SerializeField] private AudioParameterController _audioParameter;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        public void GameStart()
        {
            this.Log("MY START",Color.magenta);
            _audioParameter.InitParam();
        }
        
        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            //var value = Mathf.Clamp(Input.GetAxisRaw("Horizontal"), 0, 1);
            var value = _rigidbody.velocity.magnitude;
            _audioParameter.SetValue(value);
            this.Log(value);
        }

        private void OnDisable()
        {
            _audioParameter.SetValue(0);
        }
    }
}