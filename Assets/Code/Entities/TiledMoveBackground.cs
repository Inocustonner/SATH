using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Entities
{
    public class TiledMoveBackground : MonoBehaviour, IGameStartListener, IGameTickListener
    {
        [SerializeField] private float _speed;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private float _startPositionX;
        [SerializeField]private float _spriteWidth;

        public void GameStart()
        {
            _startPositionX = transform.position.x;
            _spriteWidth = _spriteRenderer.bounds.size.x;
        }

        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            var newPositionX = transform.position.x - _speed * Time.deltaTime;

            if (newPositionX <= _startPositionX - _spriteWidth)
            {
                newPositionX = _startPositionX;
            }

            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
        }
    }
}