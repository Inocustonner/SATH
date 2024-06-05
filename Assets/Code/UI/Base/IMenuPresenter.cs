using System;
using Code.UI.Enums;

namespace Code.UI.Base
{
    public interface IMenuPresenter
    {
        public void ChangeMenuState(MenuState state, Action onCompleted);
    }
}