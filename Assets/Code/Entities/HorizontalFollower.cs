using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Entities
{
    public class HorizontalFollower : MonoBehaviour, IGameStartListener,IGameTickListener
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _speed = 3;
        [SerializeField] private Transform _mover;
        [SerializeField] private Transform _follow;

        private Vector3 _target;


        public void GameStart()
        {
            _target = _mover.position;
        }

        public void GameTick()
        {
            if (_follow.position.x > _minX)
            {
                _target = new Vector3(_follow.position.x, _mover.position.y, _mover.position.z);
            }

            _mover.position = Vector3.Lerp(_mover.position, _target, _speed * Time.deltaTime);
        }
    }
}