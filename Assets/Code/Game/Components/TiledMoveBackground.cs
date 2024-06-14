using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components
{
    public class TiledMoveBackground : MonoBehaviour, IGameStartListener, IPartTickListener
    {
        [SerializeField] private float _speed;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private float _spriteWidth;
        private float _startPositionX;

        public void GameStart()
        {
            _startPositionX = transform.position.x;
            _spriteWidth = _spriteRenderer.bounds.size.x;
        }

        public void PartTick()
        {
            var newPositionX = transform.position.x - _speed * Time.deltaTime;

            if (newPositionX <= _startPositionX - _spriteWidth)
            {
                newPositionX = _startPositionX;
            }

            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
        }
    }
}