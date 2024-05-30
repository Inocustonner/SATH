using System;

namespace Code.Replicas.Scripts
{
    [Serializable]
    public struct ReplicaEdgeSerialized
    {
        public string SourceNode;
        public string TargetNode;
        public int Index;
    }
}