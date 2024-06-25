using Code.UI.Base;

namespace Code.UI.Menu.Destruction
{
    public class DestructionMenuPresenter: BaseMenuPresenter<DestructionMenuModel, DestructionMenuView>
    {
        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                Model.Player.Health.OnChanged += OnChangedPlayerHealth;
            }
            else
            {
                
                Model.Player.Health.OnChanged -= OnChangedPlayerHealth;
            }
        }

        private void OnChangedPlayerHealth()
        {
            View.SetPlayerHealth($"{Model.Player.Health.Current}/{Model.Player.Health.Max}");
        }
    }
}