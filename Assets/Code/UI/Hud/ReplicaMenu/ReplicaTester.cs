using Code.Data.Configs;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaTester : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private ReplicaConfig _replica;
        [SerializeField] private Lan _language;

        [Header("Components")] 
        [SerializeField] private ReplicaMenuPresenter _presenter;
        [SerializeField] private ReplicaMenuModel _model;
        [SerializeField] private ReplicaMenuView _view;

        public void StopWrite()
        {
            _view.StopWrite();
        }

        public void StartWrite()
        {
            if (!_model.IsValidating)
            {
                _presenter.ChangeMenuState(MenuState.Active);
            }

            if (_replica.TryGetLocalizedReplica(_language, out var replica))
            {
                _view.StartWrite(replica, _replica.WriteSpeed);
            }
            else
            {
                Debug.LogError("Failed to extract replica");
            }
        }
#endif
    }
}