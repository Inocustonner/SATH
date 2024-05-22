using System;
using Code.Data.Enums;

namespace Code.Scenarios.Scripts
{
    public sealed class Dialogue
    {
        private readonly DialogueConfig config;
        private DialogueNodeSerialized currentNode;
        
        public string CurrentMessage
        {
            get { return this.currentNode.Message; }
        }

        public TextCondition[] CurrentChoices
        {
            get { return this.currentNode.Conditions.ToArray(); }
        }

        public Dialogue(DialogueConfig config)
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