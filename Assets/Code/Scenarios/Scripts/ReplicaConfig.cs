using System.Collections.Generic;
using UnityEngine;

namespace Code.Scenarios.Scripts
{
    [CreateAssetMenu(fileName = "ReplicaConfig", menuName = "Config/Graph/Replica")]
    public sealed class ReplicaConfig : ScriptableObject
    {
        public List<ReplicaNodeSerialized> Nodes;
        public List<ReplicaEdgeSerialized> Edges;

        public bool TryFindStartNode(out ReplicaNodeSerialized node)
        {
            if (Nodes == null || Nodes.Count == 0)
            {
                node = default;
                return false;
            }
            return TryFindNode(Nodes[0].ID, out node);
        }

        public bool TryFindNextNode(string sourceNode, int conditionIndex, out ReplicaNodeSerialized nextNode)
        {
            for (int i = 0, count = Edges.Count; i < count; i++)
            {
                var edge = Edges[i];
                if (edge.SourceNode == sourceNode && edge.Index == conditionIndex)
                {
                    if (TryFindNode(edge.TargetNode, out nextNode))
                    {
                        return true;
                    }

                    return false;
                }
            }

            nextNode = default;
            return false;
        }

        private bool TryFindNode(string id, out ReplicaNodeSerialized result)
        {
            foreach (var node in Nodes)
            {
                if (node.ID == id)
                {
                    result = node;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}