using System;

namespace Code.Scenarios.Scripts
{
    [Serializable]
    public struct DialogueEdgeSerialized
    {
        public string SourceNode;
        public string TargetNode;
        public int Index;
    }
}