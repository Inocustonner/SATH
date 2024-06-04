using System;
using Code.Entities;
using UnityEngine;

namespace Code.Audio.AudioEvents
{
    public class CollisionAudioEvent : MonoBehaviour
    {
        [SerializeField] private CollisionObserver _collisionObserver;
        [SerializeField] private AudioEvent _audioEvent;

        private void OnCollisionEnter2D(Collision2D col)
        {
            _audioEvent.PlayAudioEvent();
        }
    }
}