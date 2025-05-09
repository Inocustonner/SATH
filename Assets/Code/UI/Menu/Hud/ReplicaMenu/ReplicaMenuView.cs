using System;
using Code.Data.DynamicData;
using Code.Data.Interfaces;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.UI.Components;
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
            onComplete?.Invoke();
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
        
        public void StartWrite(AcceleratedTextData[] replicas, AnimatedTextWaiter.Mode waitedMode)
        {
            _animatedText.StartWrite(replicas, waitedMode);
        }

        public void Skip()
        {
            _animatedText.Skip();
        }

        public void Reset()
        {
            _animatedText.ResetText();
        }

        private void AnimatedTextOnEndWrite()
        {
            OnEndWrite?.Invoke();
        }
    }
}