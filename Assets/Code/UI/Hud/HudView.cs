using System;
using Code.UI.Base;

namespace Code.UI.Hud
{
    public class HudView : BaseMenuView
    {
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
    }
}