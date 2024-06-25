using Code.Data.Enums;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.UI.Enums;
using Code.Utils;
using UnityEngine;

namespace Code.UI.Hud.TwistCurtain
{
    public class EffectsCurtainPresenter: BaseMenuPresenter<EffectsCurtainModel,EffectsCurtainView>
    {
        private EffectCurtainService _curtainService;
        private TransitionGamePartService _partTransition;

        protected override void Init()
        {
            _curtainService = Container.Instance.FindService<EffectCurtainService>();
            _partTransition = Container.Instance.FindService<TransitionGamePartService>();
        }

        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _curtainService.OnTryShowCurtain += OnTryShowCurtain;
                _partTransition.OnTransition += OnPartTransition;
            }
            else
            {
                _curtainService.OnTryShowCurtain -= OnTryShowCurtain;
                _partTransition.OnTransition -= OnPartTransition;
            }
        }

        private void OnPartTransition()
        {
            if (Model.IsValidating)
            {
                ChangeMenuState(MenuState.Inactive);
            }
        }

        private void OnTryShowCurtain(EffectCurtainType type, float duration)
        {
            this.Log("OnTryShowCurtain",Color.cyan);
            if (!Model.IsValidating)
            {
                this.Log("Change menu state ",Color.cyan);
                View.SetParams(type, duration);
                ChangeMenuState(MenuState.Active);
            }
        }
    }
}