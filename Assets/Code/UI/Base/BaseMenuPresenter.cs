using System;
using Code.Data.Interfaces;
using Code.UI.Enums;
using UnityEngine;

namespace Code.UI.Base
{
    public abstract class BaseMenuPresenter<TModel, TView> : MonoBehaviour, 
        IMenuPresenter, 
        IGameInitListener,
        IGameExitListener
        where TModel : BaseMenuModel<TModel>
        where TView : BaseMenuView
    {
        [SerializeField] protected TModel Model;
        [SerializeField] protected TView View;
        public event Action<MenuType> OnOtherMenuCall;

        public void GameInit()
        {
            Init();
            SubscribeToEvents(true);
        }


        public void GameExit()
        {
            SubscribeToEvents(false);
            Destruct();
        }

        public  void ChangeMenuState(MenuState state, Action onComplete = null)
        {
            switch (state)
            {
                case MenuState.Active:
                {
                    View.OpenMenu(onComplete: () =>
                    {
                        Model.IsValidating = true;
                        View.SetButtonsInteractable(Model.IsValidating);
                        onComplete?.Invoke();
                    });
                    break;
                }
                case MenuState.Inactive:
                {
                    Model.IsValidating = false;
                    View.SetButtonsInteractable(Model.IsValidating);
                    View.CloseMenu(() => { onComplete?.Invoke(); });
                    break;
                }
            }
        }

        protected virtual void Init() { }

        protected virtual void Destruct() { }

        protected abstract void SubscribeToEvents(bool flag);

        protected void OtherMenuCall(MenuType menuType)
        {
            OnOtherMenuCall?.Invoke(menuType);
        }

        private void OnValidate()
        {
            if (View == null && !TryGetComponent(out View))
            {
                View = gameObject.AddComponent<TView>();
            }

            if (Model == null && !TryGetComponent(out Model))
            {
                Model = gameObject.AddComponent<TModel>();
            }

            Model.IsValidating = View.IsActivatedWindow;
        }
    }
}