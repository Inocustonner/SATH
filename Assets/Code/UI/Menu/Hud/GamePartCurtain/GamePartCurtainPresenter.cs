using System.Collections;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.UI.Enums;
using Code.UI.Hud.ReplicaMenu;
using UnityEngine;

namespace Code.UI.Hud.GamePartCurtain
{
    public class GamePartCurtainPresenter: BaseMenuPresenter<GamePartCurtainModel, GamePartCurtainView>
    {
        [Header("Components")]
        [SerializeField] private ReplicaMenuModel _replicaModel;
        [Header("Services")]
        private MoveLimiter _moveLimiter;
        private TransitionGamePartService _transitionGamePart;

        private Coroutine _coroutine;

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
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(StartTransition());
        }

        private IEnumerator StartTransition()
        {
            _moveLimiter.Block();
            yield return new WaitUntil(() => !_replicaModel.IsValidating);
            ChangeMenuState(MenuState.Active, onComplete: () =>
            {
                ChangeMenuState(MenuState.Inactive);
                _moveLimiter.Unblock();
            });
        }
    }
}