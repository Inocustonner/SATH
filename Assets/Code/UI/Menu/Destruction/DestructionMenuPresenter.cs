using Code.UI.Base;
using Code.Utils;

namespace Code.UI.Menu.Destruction
{
    public class DestructionMenuPresenter: BaseMenuPresenter<DestructionMenuModel, DestructionMenuView>
    {
        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                Model.PlayerHealth.OnChanged += OnChangedPlayerHealth;
                this.Log("Subscribe");
            }
            else
            {
                
                Model.PlayerHealth.OnChanged -= OnChangedPlayerHealth;
            }
        }

        private void OnChangedPlayerHealth()
        {
            this.Log("OnChangedPlayerHealth");
            View.SetPlayerHealth($"{Model.PlayerHealth.Current}/{Model.PlayerHealth.Max}");
        }
    }
}