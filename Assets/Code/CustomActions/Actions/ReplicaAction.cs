using System;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using Code.Replicas.Scripts;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ReplicaAction : CustomAction, IGameInitListener
    {
        [SerializeField] private ReplicaConfig _replicaConfig;
        [SerializeField] private bool _isCanRepeat = true;
        private bool _isInvoked;
        public event Action<ReplicaConfig> OnTryStartReplica;

        private ReplicaService _replicaService;

        public void GameInit()
        {
            _replicaService = Container.Instance.FindService<ReplicaService>();
        }

        public override void StartAction()
        {
            if (_replicaConfig != null && IsCanInvoke())
            {
                OnTryStartReplica?.Invoke(_replicaConfig);
                _isInvoked = true;
                InvokeStartEvent();
                _replicaService.OnEndReplica += OnEndWriteReplica;
            }
        }

        private void OnEndWriteReplica()
        {
            _replicaService.OnEndReplica -= OnEndWriteReplica;
            StopAction();
        }

        public override void StopAction()
        {
            this.Log("Stop");
            InvokeEndEvent();
        }

        private bool IsCanInvoke()
        {
            return _isCanRepeat || (!_isCanRepeat && !_isInvoked);
        }
    }
}