﻿using Code.Data.DynamicData;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.Conditions;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.Replicas.Scripts;
using Code.UI.Enums;
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
        private GameConditionProvider _conditionProvider;
        private ReplicaConverter _replicaConverter;
        [Header("Debug")] 
        [SerializeField] private AcceleratedTextData[] _acceleratedTexts;

        public void GameInit()
        {
            _conditionProvider = Container.Instance.FindService<GameConditionProvider>();
        }

        public void Start()
        {
            _replicaConverter = new ReplicaConverter(_conditionProvider);
        }

        public void Skip()
        {
            _view.Skip();
        }

        public void StartWrite()
        {
            _replicaConverter.SetConfig(_replicaConfig);
            
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