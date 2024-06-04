using Code.Replicas.Scripts;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ReplicasAction : ReplicaAction
    {
        [SerializeField] private ReplicaConfig[] _allReplicas;

        private int _currentIndex;

        public override void StartAction()
        {
            Restart();
            InvokeStartActionEvent();
            InvokeStartReplicaEvent();
            _replicaService.OnEndReplica += OnEndReplica;
        }

        public override void StopAction()
        {
            if (!InProgress)
            {
                return;
            }
            _replicaService.OnEndReplica -= OnEndReplica;
            InvokeEndActionEvent();
        }

        private void OnEndReplica(ReplicaConfig obj)
        {
            TryMoveNextReplica();
        }

        private void TryMoveNextReplica()
        {
            if (_currentIndex < _allReplicas.Length - 1)
            {
                _currentIndex++;
                _replicaConfig = _allReplicas[_currentIndex];
                InvokeStartReplicaEvent();
            }
            else
            {
                StopAction();   
            }
        }

        private void Restart()
        {
            _currentIndex = 0;
            _replicaConfig = _allReplicas[_currentIndex];
        }
    }
}