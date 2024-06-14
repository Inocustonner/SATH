using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.Parts
{
    public class GamePart_2_Friendship: GamePart, IPartStartListener,IPartExitListener
    {
        public override GamePartName GamePartName => GamePartName.Part_2__friendship;

        [SerializeField] private ReplicaAction _purpleReplica;

        public bool IsSpeakWithPurple { get; private set; }

        public void PartStart()
        {
            SubscribeToEvents(true);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        public override void Restart()
        {
            IsSpeakWithPurple = false;
            InvokeUpdateDataEvent();
            InvokeRestartEvent();
            base.Restart();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _purpleReplica.OnEnd += OnEndPurpleReplica;
            }
            else
            {
                _purpleReplica.OnEnd -= OnEndPurpleReplica;
            }
        }

        private void OnEndPurpleReplica()
        {
            IsSpeakWithPurple = true;
            InvokeUpdateDataEvent();
        }
    }
}