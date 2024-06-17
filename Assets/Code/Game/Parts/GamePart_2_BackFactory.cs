using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.Components.Factory;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.Parts
{
    public class GamePart_2_BackFactory : GamePart, IPartTickListener
    {
        public override GamePartName GamePartName => GamePartName.Part_2__back_to_factory;
        [Header("Components")]
        [SerializeField] private ReplicaNumerableAction _replicasAction;
        [SerializeField] private PushListener _pushListener;
        
        [Header("Static value")] 
        private const int MAX_ID = 14;
        
        [Header("Dynamic value")]
        private int _id;
        private Coroutine _transitionCoroutine;

        
        public void PartTick()
        {
            var pushTime = _pushListener.GetPushTime(); 
            if (pushTime / 10 > _id && _id < MAX_ID)
            {
                _replicasAction.SetID(_id);
                _replicasAction.StartAction();
                _id++;
            }

        }
    }
}