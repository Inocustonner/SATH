using System;
using Code.Data.Enums;

namespace Code.Scenarios.Scripts
{
    public sealed class Replica
    {
        private readonly ReplicaConfig config;
        private ReplicaNodeSerialized currentNode;
        
        public string CurrentMessage
        {
            get { return /* this.currentNode.Message;*/""; }
        }

        public ReplicaCondition[] CurrentConditions
        {
            get { return this.currentNode.Conditions.ToArray(); }
        }

        public Replica(ReplicaConfig config)
        {
            if (!config.FindStartNode(out var node))
            {
                throw new Exception("Entry point is absent!");
            }

            this.config = config;
            this.currentNode = node;
        }

        public bool MoveNext(int choiceIndex)
        {
            if (this.config.FindNextNode(this.currentNode.ID, choiceIndex, out var nextNode))
            {
                this.currentNode = nextNode;
                return true;
            }

            return false;
        }
    }
}