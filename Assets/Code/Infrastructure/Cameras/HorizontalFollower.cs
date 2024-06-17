using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Infrastructure.Cameras
{
    public class HorizontalFollower : MonoBehaviour, IGameStartListener, IPartTickListener, IRestarable 
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _speed = 3;
        [SerializeField] private Transform _mover;
        [SerializeField] private Transform _follow;

        private Vector3 _target;
        private Vector3 _startPosition;
        
        public void GameStart()
        {
            _startPosition = _mover.position;
            _target = _mover.position;
        }

        public void PartTick()
        {
            if (_follow.position.x > _minX)
            {
                _target = new Vector3(_follow.position.x, _mover.position.y, _mover.position.z);
            }

            _mover.position = Vector3.Lerp(_mover.position, _target, _speed * Time.deltaTime);
        }

        public void Restart()
        {
            _mover.position = _startPosition;
        }
    }
}