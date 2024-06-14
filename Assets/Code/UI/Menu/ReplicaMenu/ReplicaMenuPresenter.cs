using System;
using Code.Data.DynamicData;
using Code.Data.Interfaces;
using Code.Game.Components;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.UI.Enums;
using Code.Utils;
using UnityEngine;

namespace Code.UI.Menu.ReplicaMenu
{
    public class ReplicaMenuPresenter : BaseMenuPresenter<ReplicaMenuModel, ReplicaMenuView>, IGameTickListener
    {
        [Header("Services")] 
        private ReplicaService _replicaService;
        private CameraService _cameraService;

        private Vector3 _offset;
        private event Action OnEndWriteReplicas;

        protected override void Init()
        {
            _replicaService = Container.Instance.FindService<ReplicaService>();
            _cameraService = Container.Instance.FindService<CameraService>();
            _offset = transform.position;
        }

        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _replicaService.OnStartReplica += OnStartReplica;
                _replicaService.OnStopReplicaPart += OnStopReplicaPart;
                _replicaService.OnSwitchReplicaLanguage += OnSwitchLanguage;
                View.OnEndWrite += OnEndWrite;
            }
            else
            {
                _replicaService.OnStartReplica -= OnStartReplica;
                _replicaService.OnStopReplicaPart -= OnStopReplicaPart;
                _replicaService.OnSwitchReplicaLanguage -= OnSwitchLanguage;
                View.OnEndWrite -= OnEndWrite;
            }
        }

        public void GameTick()
        {
            if (Model.IsValidating && _cameraService.IsMovement)
            {
                SetPosition();
            }
        }

        private void SetPosition()
        {
            var newPos = _cameraService.CurrentCamera.transform.position + _offset;
            newPos.z = 0;
            transform.position = newPos;
        }

        private void OnStartReplica(AcceleratedTextData[] replicas, AnimatedTextWaiter.Mode waitedMode, Action action)
        {
            Model.ReplicasCount++;

            if (Model.ReplicasCount > 1)
            {
                View.Skip();
                View.Reset();
                Model.ReplicasCount--;
                OnEndWriteReplicas?.Invoke();
            }
            else
            {
                Model.IsTyping = true;
            }

            if (!Model.IsValidating)
            {
                SetPosition();
                ChangeMenuState(MenuState.Active);
            }

            OnEndWriteReplicas = action;
            View.StartWrite(replicas, waitedMode);
        }

        private void OnSwitchLanguage(AcceleratedTextData[] replicas, AnimatedTextWaiter.Mode waitedMode)
        {
            View.StartWrite(replicas, waitedMode);
        }

        private void OnEndWrite()
        {
            Model.ReplicasCount--;
            OnEndWriteReplicas?.Invoke();
            if (Model.ReplicasCount == 0 )
            {
                Model.IsTyping = false;
                ChangeMenuState(MenuState.Inactive);       
                View.Reset();
            }
        }

        private void OnStopReplicaPart()
        {
            View.Skip();
        }
    }
}