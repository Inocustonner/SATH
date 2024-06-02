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
        [SerializeField] protected ReplicaConfig _replicaConfig;
        public event Action<ReplicaConfig> OnTryStartReplica;

        private ReplicaService _replicaService;

        public void GameInit()
        {
            _replicaService = Container.Instance.FindService<ReplicaService>();
        }

        public override void StartAction()
        {
            if (_replicaConfig != null)
            {
                this.Log($"Start action {_replicaConfig.name}", Color.cyan);
                OnTryStartReplica?.Invoke(_replicaConfig);
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
            this.Log($"End action {_replicaConfig.name}", Color.cyan);
            InvokeEndEvent();
        }
    }
}