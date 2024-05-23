using System;
using System.Linq;
using Code.Data.Configs;
using Code.Data.Enums;
using Core.Infrastructure.Utils;

namespace Code.Scenarios.Scripts
{
    public sealed class Replica
    {
        private readonly ReplicaConfig _config;
        private ReplicaNodeSerialized _currentNode;
        
        public bool  TryGetCurrentConditions(out GameCondition[] conditions)
        {
            conditions =  _currentNode.Conditions.ToArray();
            return conditions != null || conditions.Length > 0;
        }

        public Replica(ReplicaConfig config)
        {
            if (!config.TryFindStartNode(out var node))
            {
                throw new Exception("Entry point is absent!");
            }

            _config = config;
            _currentNode = node;
        }

        public bool TryGetCurrentMessage(Lan language, out string replica)
        {
            replica = "";
            var localization = _currentNode.Localization.FirstOrDefault(l => l.Language == language);
            if (localization.Parts != null && localization.Parts.Count > 0)
            {
                foreach (var part in localization.Parts)
                {
                    replica += part.MessageText + " ";
                }
                return true;
            }

            return false;
        }
        public bool MoveNext(int conditionIndex)
        { 
            if (_config.TryFindNextNode(_currentNode.ID, conditionIndex, out var nextNode))
            {
                _currentNode = nextNode;
                this.Log($"реплика смогла перейти на следующую часть {_currentNode.ID}");
                return true;
            }

            return false;
        }
    }
}