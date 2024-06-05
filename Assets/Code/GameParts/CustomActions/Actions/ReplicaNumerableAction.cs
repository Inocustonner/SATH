using Code.Replicas.Scripts;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions
{
    public class ReplicaNumerableAction : ReplicaAction
    {
        [SerializeField] private int _currentID;
        [SerializeField] private ReplicaConfig[] _replicas;

        public void SetID(int id)
        {
            _currentID = id;
            if (_currentID < _replicas.Length)
            {
                _replicaConfig = _replicas[_currentID];
            }
        }

        private void OnValidate()
        {
            if (_currentID < _replicas.Length)
            {
                _replicaConfig = _replicas[_currentID];
            }
        }
    }
}