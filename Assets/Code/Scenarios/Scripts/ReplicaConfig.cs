using System.Collections.Generic;
using UnityEngine;

namespace Code.Scenarios.Scripts
{
    [CreateAssetMenu(
        fileName = "DialogueConfig",
        menuName = "Lessons/New DialogueConfig"
    )]
    public sealed class ReplicaConfig : ScriptableObject
    {
        public string startNode;
        public List<ReplicaNodeSerialized> nodes;
        public List<ReplicaEdgeSerialized> edges;

        public bool FindStartNode(out ReplicaNodeSerialized node)
        {
            var nodeID = startNode != "" ? startNode : nodes[0].ID;
            return FindNode(nodeID, out node);
        }

        private bool FindNode(string id, out ReplicaNodeSerialized result)
        {
            foreach (var node in this.nodes)
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

        public bool FindNextNode(string sourceNode, int choiceIndex, out ReplicaNodeSerialized nextNode)
        {
            for (int i = 0, count = this.edges.Count; i < count; i++)
            {
                var edge = this.edges[i];
                if (edge.SourceNode == sourceNode && edge.Index == choiceIndex)
                {
                    if (this.FindNode(edge.TargetNode, out nextNode))
                    {
                        return true;
                    }

                    return false;
                }
            }

            nextNode = default;
            return false;
        }
    }
}