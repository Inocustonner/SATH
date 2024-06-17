using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Factory.Back
{
    public class WorkerSpawnMover: MonoBehaviour, IPartTickListener
    {
        [SerializeField] private Rigidbody2D _follow;
        [SerializeField] private FactoryWorkersPool _workersPool;
        [SerializeField] private float _offset = 50;
        
        private float _maxLeftPosition;

        public void PartTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (_follow.velocity.x < 0)
            {
                if (_maxLeftPosition - _offset < _follow.position.x)
                {
                    _maxLeftPosition = _follow.transform.position.x;
                    _workersPool.SetSpawnPositionX(_maxLeftPosition - _offset);
                }
            }
        }
    }
}