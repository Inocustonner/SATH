using System;

namespace Code.UI.Base
{
    public interface IMenuPresenter
    {
        public void ChangeMenuState(MenuState state, Action onCompleted);
    }
}