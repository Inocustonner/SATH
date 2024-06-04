using Code.Data.Enums;
using Code.Infrastructure.DI;
using Code.Replicas;

namespace Code.Infrastructure.Services
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