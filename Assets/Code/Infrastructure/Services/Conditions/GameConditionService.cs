using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class GameConditionService : IService, IGameInitListener
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
                this.Log($"{conditions[i]} {observer != null}", Color.yellow);
            }
        }

        public bool GetValue(GameCondition condition)
        {
            this.Log($"{condition} -> " +
                     $"(null = {_conditions[condition] == null }) " +
                     $"|| (is true = {_conditions[condition]?.IsTrue}) " +
                     $"== {_conditions[condition] == null || _conditions[condition].IsTrue}", Color.yellow);
            
            return _conditions[condition] == null || _conditions[condition].IsTrue;
        }
        
  
    }
}