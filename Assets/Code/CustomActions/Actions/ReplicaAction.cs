using System;
using Code.Scenarios.Scripts;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ReplicaAction : CustomAction
    {
        [SerializeField] private ReplicaConfig _replicaConfig;
        [SerializeField] private bool _isCanRepeat = true;
        private bool _isInvoked;
        public event Action<ReplicaConfig> OnTryStartReplica;
        
        public override void StartAction()
        {
            if (_replicaConfig != null && IsCanInvoke())
            {
                OnTryStartReplica?.Invoke(_replicaConfig);
                _isInvoked = true;
            }
        }

        public override void StopAction()
        {
        }

        private bool IsCanInvoke()
        {
            return _isCanRepeat || (!_isCanRepeat && !_isInvoked);
        }
    }
}