using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.Components.Factory.Back
{
    public class PlayerMoveLeftObserver : MonoBehaviour, IPartTickListener, IRestarable
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private ReplicaNumerableAction _replicasAction;

        private float _currentMoveTime;
        private int _replicaId;
        private const int MAX_ID = 14;

        public void PartTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (_rigidbody2D.velocity.x < 0)
            {
                _currentMoveTime += Time.deltaTime;
                if (_currentMoveTime / 10 > _replicaId && _replicaId < MAX_ID)
                {
                    _replicasAction.SetID(_replicaId);
                    _replicasAction.StartAction();
                    _replicaId++;
                }
            }
        }

        public void Restart()
        {
            _currentMoveTime = 0;
        }
    }
}