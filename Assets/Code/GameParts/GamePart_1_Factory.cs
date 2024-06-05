using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.GameParts.Components;
using Code.GameParts.CustomActions.Actions;
using UnityEngine;

namespace Code.GameParts
{
    public class GamePart_1_Factory: GamePart, IPartStartListener, IPartTickListener
    {
        public override GamePartName GamePartName => GamePartName.Part_1__factory;
        [SerializeField] private ReplicaNumerableAction _replicasAction;
        [SerializeField] private PushListener _pushListener;

        private int id;
        public void PartStart()
        {
          //  _replicasAction.StartAction();    
        }

        public void PartTick()
        {
            var pushTime = _pushListener.GetPushTime(); 
            if (pushTime / 10 > id)
            {
                _replicasAction.SetID(id);
                _replicasAction.StartAction();
                id++;
            }
        }
    }
}