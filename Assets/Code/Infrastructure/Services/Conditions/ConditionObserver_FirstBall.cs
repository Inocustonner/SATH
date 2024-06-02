using Code.Data.Enums;
using Code.Infrastructure.DI;
using Code.Replicas;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class ConditionObserver_FirstBall : ConditionObserver
    {
        public override GameCondition Condition => GameCondition.FirstBall;
        public override bool IsTrue { get; protected set; }
        
        private GamePart_School _schoolPart;

        public override void GameInit()
        {
            _schoolPart = Container.Instance.FindGamePart<GamePart_School>();
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
            IsTrue = _schoolPart.AttemptNumber == 0;
            this.Log($"{IsTrue}",Color.magenta);
        }
    }
}