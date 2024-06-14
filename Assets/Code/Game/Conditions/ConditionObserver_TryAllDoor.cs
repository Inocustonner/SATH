using Code.Data.Enums;
using Code.Game.Parts;
using Code.Infrastructure.DI;

namespace Code.Game.Conditions
{
    public class ConditionObserver_TryAllDoor : ConditionObserver
    {
        public override GameCondition Condition => GameCondition.TryAllDoor;
        public override bool IsTrue { get; protected set; }

        private GamePart_1_Home _homePart;

        public override void GameInit()
        {
            _homePart = Container.Instance.FindGamePart<GamePart_1_Home>();
        }

        public override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _homePart.OnUpdatePartData += RefreshState;
            }
            else
            {
                _homePart.OnUpdatePartData -= RefreshState;
                
            }
        }
        
        public override void RefreshState()
        {
            IsTrue = _homePart.IsTryOpenAllDoor();
        }
    }
}