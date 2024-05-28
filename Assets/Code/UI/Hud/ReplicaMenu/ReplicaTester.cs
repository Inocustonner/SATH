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
    public class ReplicaTester : MonoBehaviour, IGameInitListener
    {
        [SerializeField] private ReplicaConfig _replicaConfig;
        [SerializeField] private Lan _language;
        [Header("Components")] 
        [SerializeField] private ReplicaMenuPresenter _presenter;
        [SerializeField] private ReplicaMenuModel _model;
        [SerializeField] private ReplicaMenuView _view;
        [Header("Service")]
        private GameConditionService _conditionService;
        private ReplicaConverter _replicaConverter;
        [Header("Debug")] 
        [SerializeField] private AcceleratedTextData[] _acceleratedTexts;

        public void GameInit()
        {
            _conditionService = Container.Instance.FindService<GameConditionService>();
        }

        public void Start()
        {
            _replicaConverter = new ReplicaConverter(_conditionService);
            _replicaConverter.SetConfig(_replicaConfig);
        }

        public void Skip()
        {
            _view.Skip();
        }

        public void StartWrite()
        {
            if (!_model.IsValidating)
            {
                _presenter.ChangeMenuState(MenuState.Active);
            }

            if (_replicaConverter.TryGetAcceleratedTexts(_language, out var acceleratedText))
            {
                var waitedMode = _replicaConfig.IsBlockMovement
                    ? AnimatedTextWaiter.Mode.PressKey
                    : AnimatedTextWaiter.Mode.Time;

                _acceleratedTexts = acceleratedText;
                _view.StartWrite(acceleratedText, waitedMode);
            }
            else
            {
                Debug.Log($"Suck my dick bitch");
            }
        }
    }
}