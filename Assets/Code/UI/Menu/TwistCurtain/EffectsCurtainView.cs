using System;
using System.Collections.Generic;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.Materials;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Menu.TwistCurtain
{
    public class EffectsCurtainView: BaseMenuView, IGameInitListener, IGameTickListener
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TwistUvMaterialController _twistUvMaterialController;

        [Header("Static data")] 
        [SerializeField] private float _showSpeed; 
        
        [Header("Dynamic data")] 
        [SerializeField] private EffectCurtainType _effectCurtainType;
        [SerializeField] private float _currentDuration;
         private BaseMaterialController _currentController;
        
        private Dictionary<EffectCurtainType, BaseMaterialController> _controllers;
        

        public void GameInit()
        {
            _controllers = new()
            {
                { EffectCurtainType.Twist , _twistUvMaterialController}
            };
            _twistUvMaterialController.Init(Container.Instance.FindService<CoroutineRunner>());
        }

        public void SetParams(EffectCurtainType type, float duration)
        {
            _effectCurtainType = type;
            _currentDuration = duration;
            _currentController = _controllers[type];
        }

        public override void OpenMenu(Action onComplete = null)
        {
            _canvasGroup.alpha = 0;
            windowTransform.gameObject.SetActive(true);
            switch (_effectCurtainType)
            {
                case EffectCurtainType.None:
                    break;
                case EffectCurtainType.Twist:
                    _twistUvMaterialController.StartPlay();
                    break;
            }   
            onComplete?.Invoke();
        }

        public override void CloseMenu(Action onComplete = null)
        {
            switch (_effectCurtainType)
            {
                case EffectCurtainType.None:
                    break;
                case EffectCurtainType.Twist:
                    _twistUvMaterialController.StopPlay();
                    break;
            }   
            _canvasGroup.alpha = 0;
            windowTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        public void GameTick()
        {
            if (!windowTransform.gameObject.activeInHierarchy)
            {
                return;
            }

            if (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += _showSpeed * Time.deltaTime;
            }
        }
    }
}