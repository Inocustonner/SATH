using System;
using System.Collections.Generic;
using Code.Data.DynamicData;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaMenuView : BaseMenuView, IGameInitListener
    {
        [SerializeField] private AnimatedText _animatedText;
        public event Action OnEndWrite;

        public void GameInit()
        {
            _animatedText.OnEndWrite += AnimatedTextOnEndWrite;
        }

        public override void OpenMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(true);
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
        }
        
        public void StartWrite(AcceleratedTextData[] replicas, AnimatedTextWaiter.Mode waitedMode)
        {
            _animatedText.StartWrite(replicas, waitedMode);
        }

        public void Skip()
        {
            _animatedText.Skip();
            this.Log("Skip",Color.red);
        }

        public void Reset()
        {
            _animatedText.ResetText();
        }

        private void AnimatedTextOnEndWrite()
        {
            this.Log("On End",Color.red);
            OnEndWrite?.Invoke();
        }
    }
}