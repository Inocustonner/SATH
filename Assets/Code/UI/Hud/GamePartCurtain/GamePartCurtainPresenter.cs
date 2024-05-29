using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.Hud.GamePartCurtain
{
    public class GamePartCurtainPresenter: BaseMenuPresenter<GamePartCurtainModel, GamePartCurtainView>
    {
        [Header("Services")]
        private MoveLimiter _moveLimiter;
        private TransitionGamePartService _transitionGamePart;
        

        protected override void Init()
        {
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
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
            _moveLimiter.Block();
            ChangeMenuState(MenuState.Active, onComplete: () =>
            {
                ChangeMenuState(MenuState.Inactive);
                _moveLimiter.Unblock();
            });
        }
    }
}