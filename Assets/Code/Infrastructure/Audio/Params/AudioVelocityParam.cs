using System;
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
            _audioParameter.InitParam();
        }

        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            this.Log(_rigidbody.velocity.magnitude);
            _audioParameter.ChangeParam(_rigidbody.velocity.magnitude);
        }

        private void OnDisable()
        {
            _audioParameter.ChangeParam(0);
        }
    }
}