using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components
{
    public class ParallaxBackground : MonoBehaviour, IGameStartListener, IPartTickListener
    {
        [SerializeField] private Transform _cam; 
        [SerializeField] private float _parallaxEffect;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _length, _startPosition;

        public void GameStart()
        {
            _startPosition = transform.position.x;
            _length = _spriteRenderer.bounds.size.x;
        }

        public void PartTick()
        {
            var temp = (_cam.transform.position.x * (1 - _parallaxEffect));
            var dist = (_cam.transform.position.x * _parallaxEffect);

            transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);

            if (temp > _startPosition + _length)
            {
                _startPosition += _length;
            }
            else if (temp < _startPosition - _length)
            {
                _startPosition -= _length;
            }
        }

    }
}