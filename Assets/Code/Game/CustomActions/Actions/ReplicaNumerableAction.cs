using System;
using Code.Replicas.Scripts;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class ReplicaNumerableAction : ReplicaAction
    {
        [SerializeField] private int _currentID;
        [SerializeField] private ReplicaConfig[] _replicas;
        public event Action OnSetLastReplica;

        public void SetID(int id)
        {
            _currentID = id;
            if (_currentID < _replicas.Length)
            {
                _replicaConfig = _replicas[_currentID];
                if (id == _replicas.Length - 1)
                {
                    OnSetLastReplica?.Invoke();
                }
            }
        }

        private void OnValidate()
        {
            if (_currentID < _replicas.Length)
            {
                _replicaConfig = _replicas[_currentID];
            }
        }

        public int GetReplicasCount()
        {
            return _replicas.Length;
        }
    }
}