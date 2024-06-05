using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;

namespace Code.Infrastructure.Services.Conditions
{
    public class GameConditionProvider : IService, IGameInitListener
    {
        private ConditionObserver[] _conditionObservers;
        private readonly Dictionary<GameCondition, ConditionObserver> _conditions = new();

        public void GameInit()
        {
            _conditionObservers = Container.Instance.GetContainerComponents<ConditionObserver>();
            var conditions = Enum.GetValues(typeof(GameCondition)).Cast<GameCondition>().ToArray();
            for (int i = 0; i < conditions.Length; i++)
            {
                var observer = _conditionObservers.FirstOrDefault(c => c.Condition == conditions[i]);
                _conditions.Add(conditions[i],observer);
            }
        }

        public bool GetValue(GameCondition condition)
        {
            return _conditions[condition] == null || _conditions[condition].IsTrue;
        }
    }
}