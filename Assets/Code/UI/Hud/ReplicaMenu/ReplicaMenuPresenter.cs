using System;
using System.Collections;
using Code.Data.Configs;
using Code.Infrastructure.DI;
using Code.Services;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaMenuPresenter : BaseMenuPresenter<ReplicaMenuModel, ReplicaMenuView>
    {
        [Header("Services")]
        private ReplicaService _replicaService;
        [Header("Static data")]
        private float _disableDelay;
        [Header("Static data")] 
        private Coroutine _disableCoroutine;


        protected override void Init()
        {
            _replicaService = Container.Instance.FindService<ReplicaService>();
            _disableDelay = Container.Instance.FindConfig<UIConfig>().ReplicaDisableDelay;
        }

        protected override void Destruct()
        {
            TryStopCoroutine();
        }

        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _replicaService.OnStartReplica += OnStartReplica;
                _replicaService.OnStopReplica += OnStopReplica;
                View.OnEndWrite += OnEndWrite;
            }
            else
            {
                _replicaService.OnStartReplica -= OnStartReplica;
                _replicaService.OnStopReplica -= OnStopReplica;
                View.OnEndWrite -= OnEndWrite;
            }
        }

        private void OnStartReplica(string replica, float speed,Action action)
        {
            if (!Model.IsValidating)
            {
                ChangeMenuState(MenuState.Active);
            }
       
            View.StartWrite(replica,speed);
        }

        private void OnEndWrite()
        {
            TryStopCoroutine();
            _disableCoroutine = StartCoroutine(DisableWithDelay());
        }

        private void OnStopReplica()
        {
            View.StopWrite();
        }

        private IEnumerator DisableWithDelay()
        {
            yield return new WaitForSeconds(_disableDelay);
            ChangeMenuState(MenuState.Inactive);
        }

        private void TryStopCoroutine()
        {
            if (_disableCoroutine != null)
            {
                StopCoroutine(_disableCoroutine);
            }
        }
    }
}