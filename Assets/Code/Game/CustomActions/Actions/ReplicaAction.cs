using System;
using Code.Data.Interfaces;
using Code.Infrastructure.Audio.AudioEvents;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.Replicas.Scripts;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class ReplicaAction : CustomAction, IGameInitListener
    {
        [SerializeField] protected ReplicaConfig _replicaConfig;
        [SerializeField] protected AudioEvent _voice;

        [Header("Debug")] 
        [TextArea, SerializeField] private string _text;
        
        protected ReplicaService _replicaService;
        public event Action<ReplicaConfig> OnTryStartReplica;

        public void GameInit()
        {
            _replicaService = Container.Instance.FindService<ReplicaService>();
        }

        public override void StartAction()
        {
            if (_replicaConfig != null)
            {
                _voice.PlayAudioEvent();
                InvokeStartReplicaEvent();
                InvokeStartActionEvent();
                _replicaService.OnEndReplica += OnEndWriteReplica;
            }
        }

        public override void StopAction()
        {
            if (!InProgress)
            {
                return;
            }

            InvokeEndActionEvent();
        }

        protected virtual void OnEndWriteReplica(ReplicaConfig replicaConfig)
        {
            if (replicaConfig == _replicaConfig)
            {
                _replicaService.OnEndReplica -= OnEndWriteReplica;
                StopAction();
            }
        }

        protected void InvokeStartReplicaEvent()
        {
            OnTryStartReplica?.Invoke(_replicaConfig);
        }

        private void OnValidate()
        {
            if (_replicaConfig != null)
            {
                _text = _replicaConfig.DebugText;
            }
        }
    }
}