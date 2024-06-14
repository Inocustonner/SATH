using Code.Data.Enums;
using Code.Game.Parts;
using Code.Infrastructure.DI;

namespace Code.Game.Conditions
{
    public class ConditionObserver_SpeakWithPurple : ConditionObserver
    {
        private GamePart_2_Friendship _gamePart;
        public override GameCondition Condition => GameCondition.SpeakWithPurple;
        public override bool IsTrue { get; protected set; }

        public override void GameInit()
        {
            _gamePart = Container.Instance.FindGamePart<GamePart_2_Friendship>();
        }

        public override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _gamePart.OnUpdatePartData += RefreshState;
            }
            else
            {
                _gamePart.OnUpdatePartData -= RefreshState;
            }
        }

        public override void RefreshState()
        {
            IsTrue = _gamePart.IsSpeakWithPurple;
        }
    }
}