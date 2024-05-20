using System;
using Code.Infrastructure.GameLoop;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaMenuView : BaseMenuView, IGameInitListener
    {
        [SerializeField] private AnimatedText _animatedText;
        public event Action OnEndWrite;

        public void GameInit()
        {
            _animatedText.OnEndWrite += AnimatedTextOnOnEndWrite;
        }

        public override void OpenMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(true);
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
        }

        public void StartWrite(string replica, float speed)
        {
            _animatedText.StartWrite(replica, speed);
        }

        public void StopWrite()
        {
            _animatedText.StopWrite();
        }

        private void AnimatedTextOnOnEndWrite()
        {
            OnEndWrite?.Invoke();
        }
    }
}