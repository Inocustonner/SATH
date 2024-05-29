using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.GamePartCurtain
{
    public class GamePartCurtainPresenter: BaseMenuPresenter<GamePartCurtainModel, GamePartCurtainView>
    {
        [Header("Services")]
        private InputLimiter _inputLimiter;
        private TransitionGamePartService _transitionGamePart;
        

        protected override void Init()
        {
            _inputLimiter = Container.Instance.FindService<InputLimiter>();
            _transitionGamePart = Container.Instance.FindService<TransitionGamePartService>();
            base.Init();
        }

        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                 _transitionGamePart.OnStartTransition += OnStartTransition;   
            }
            else
            {
                _transitionGamePart.OnStartTransition -= OnStartTransition;   
            }
        }

        private void OnStartTransition()
        {
            _inputLimiter.Block();
            ChangeMenuState(MenuState.Active, onComplete: () =>
            {
                ChangeMenuState(MenuState.Inactive);
                _inputLimiter.Unblock();
            });
        }
    }
}