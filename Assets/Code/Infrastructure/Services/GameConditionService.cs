using System;
using System.Collections.Generic;
using System.Linq;
using Code.CustomActions.Actions;
using Code.Data.Enums;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    [Serializable]
    public struct ConditionActionData
    {
        public GameCondition Condition;
        public CustomAction Action;
    }

    public class GameConditionService : MonoBehaviour, IService, IGameInitListener, IGameStartListener
    {
        [SerializeField] private ConditionActionData[] _conditionActions;

        private readonly Dictionary<GameCondition, bool> _conditions = new();

        public void GameInit()
        {
            var conditions = Enum.GetValues(typeof(GameCondition)).Cast<GameCondition>().ToArray();
            for (int i = 0; i < conditions.Length; i++)
            {
                _conditions.Add(conditions[i], false);
            }
            _conditions[GameCondition.None] = true;
        }

        public void GameStart()
        {
            SubscribeToEvents();
        }

        public bool GetValue(GameCondition condition)
        {
            return _conditions.ContainsKey(condition) && _conditions[condition];
        }

        private void SubscribeToEvents()
        {
            for (int i = 0; i < _conditionActions.Length; i++)
            {
                _conditionActions[i].Action.OnEnd += () =>
                {
                    _conditions[_conditionActions[i].Condition] = true;
                };
            }
        }
    }
}