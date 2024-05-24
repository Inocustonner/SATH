using System;

namespace Code.Scenarios.Scripts
{
    [Serializable]
    public struct ReplicaEdgeSerialized
    {
        public string SourceNode;
        public string TargetNode;
        public int Index;
    }
}