using System.Collections.Generic;
using Code.Data.Configs;
using Code.Data.DynamicData;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using Code.Scenarios.Scripts;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaTester : MonoBehaviour, IGameInitListener/*, IGameStartListener*/
    {
        [SerializeField] private ReplicaConfig _replicaConfig;
        [SerializeField] private Lan _language;
        [Header("Components")] 
        [SerializeField] private ReplicaMenuPresenter _presenter;
        [SerializeField] private ReplicaMenuModel _model;
        [SerializeField] private ReplicaMenuView _view;
        [Header("Service")] 
        private GameConditionService _conditionService;
        
       private Replica _replica;
       [Header("Debug")] 
       [SerializeField] private AcceleratedText[] _acceleratedTexts;
        public void GameInit()
        {
            _conditionService = Container.Instance.FindService<GameConditionService>();
        }

        public void Start()
        {
            _replica = new Replica(_replicaConfig, _conditionService);
            
        }

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

            if (_replica.TryGetListAcceleratedText(_language, out var acceleratedText))
            {
                _acceleratedTexts = acceleratedText.ToArray();
                _view.StartWrite(_acceleratedTexts);
            }
            else
            {
                Debug.Log($"Suck my dick bitch");
            }
        }
        
    }
}