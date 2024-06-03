using System;
using Code.Data.DynamicData;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Core.Infrastructure.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaMenuPresenter : BaseMenuPresenter<ReplicaMenuModel, ReplicaMenuView>
    {
        [Header("Services")]
        private ReplicaService _replicaService;
        private event Action OnEndWriteReplicas;

        protected override void Init()
        {
            _replicaService = Container.Instance.FindService<ReplicaService>();
        }
        
        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _replicaService.OnStartReplica += OnStartReplica;
                _replicaService.OnStopReplicaPart += OnStopReplicaPart;
                View.OnEndWrite += OnEndWrite;
            }
            else
            {
                _replicaService.OnStartReplica -= OnStartReplica;
                _replicaService.OnStopReplicaPart -= OnStopReplicaPart;
                View.OnEndWrite -= OnEndWrite;
            }
        }

        private void OnStartReplica(AcceleratedTextData[] replicas, AnimatedTextWaiter.Mode waitedMode, Action action)
        {

            if (Model.IsTyping)
            {
                View.Skip();
                View.Reset();
                /*Model.ReplicasCount--;
                OnEndWriteReplicas?.Invoke();*/
            }
            else
            {
                Model.IsTyping = true;
            }

            if (!Model.IsValidating)
            {
                ChangeMenuState(MenuState.Active);
                this.Log("active",Color.red);
            }

            Model.ReplicasCount++;
            OnEndWriteReplicas = action;
            View.StartWrite(replicas,waitedMode);
        }

        private void OnEndWrite()
        {
            Model.ReplicasCount--;
            OnEndWriteReplicas?.Invoke();
            if (Model.ReplicasCount == 0)
            {
                Model.IsTyping = false;
                ChangeMenuState(MenuState.Inactive);
                this.Log("inactive",Color.red);
                //View.Reset();
            }
        }

        private void OnStopReplicaPart()
        {
            View.Skip();
        }
    }
}