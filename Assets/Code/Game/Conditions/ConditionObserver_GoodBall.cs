using Code.Data.Enums;
using Code.Game.Parts;
using Code.Infrastructure.DI;

namespace Code.Game.Conditions
{
    public class ConditionObserver_GoodBall : ConditionObserver
    {
        public override GameCondition Condition => GameCondition.GoodBall;
        public override bool IsTrue { get; protected set; }
        
        private GamePart_1_School _schoolPart;

        public override void GameInit()
        {
            _schoolPart = Container.Instance.FindGamePart<GamePart_1_School>();
        }

        public override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _schoolPart.OnUpdatePartData += RefreshState;
            }
            else
            {
                _schoolPart.OnUpdatePartData -= RefreshState;
            }
        }
        

        public override void RefreshState()
        {
            IsTrue = _schoolPart.IsWin && _schoolPart.AttemptNumber == 0;
        }
    }
}