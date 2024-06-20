using System;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using Code.Infrastructure.DI;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class EffectCurtainService:IService, IGameInitListener, IGameStartListener, IGameExitListener 
    {
        private EffectCurtainAction[] _actions;

        public event Action<EffectCurtainType, float> OnTryShowCurtain;
        public void GameInit()
        {
            _actions = Container.Instance.GetContainerComponents<EffectCurtainAction>();
            this.Log($"actions count {_actions.Length}",Color.cyan);
        }

        public void GameStart()
        {
            SubscribeToEvents(true);
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                foreach (var curtainAction in _actions)
                {
                    curtainAction.OnTryShowEffectCurtain += OnTryShowEffectCurtain;                    
                }
            }
            else
            {
                foreach (var curtainAction in _actions)
                {
                    curtainAction.OnTryShowEffectCurtain -= OnTryShowEffectCurtain;                    
                }
            }
        }

        private void OnTryShowEffectCurtain(EffectCurtainType arg1, float arg2)
        {
            this.Log($"On try show",Color.cyan);
            OnTryShowCurtain?.Invoke(arg1,arg2);
        }
    }
}