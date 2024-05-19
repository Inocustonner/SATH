using System;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.ReplicaMenu
{
    public class ReplicaMenuView : BaseMenuView
    {
        [SerializeField] private AnimatedText _animatedText;
        public override void OpenMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(true);
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
        }
        
        public void SetReplica(string replica)
        {
            _animatedText.StartWrite(replica);
        }

        public void StopWrite()
        {
            _animatedText.StopWrite();
        }
    }
}