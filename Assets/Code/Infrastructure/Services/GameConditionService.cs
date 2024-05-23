using System.Collections.Generic;
using Code.Data.Enums;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class GameConditionService : IService
    {
        private Dictionary<GameCondition, bool> _conditions = new Dictionary<GameCondition, bool>()
        {
            { GameCondition.None, true },
            { GameCondition.EnterWhiteRoom, false },
            { GameCondition.KillMePlease, true },
        };

        public bool GetValue(GameCondition condition)
        {
            return _conditions.ContainsKey(condition) && _conditions[condition];
        }
    }
}