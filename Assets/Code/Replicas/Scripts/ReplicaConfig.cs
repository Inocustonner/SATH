using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Replicas.Scripts
{
    [CreateAssetMenu(fileName = "ReplicaConfig", menuName = "Config/Graph/Replica")]
    public sealed class ReplicaConfig : ScriptableObject
    {
        public bool IsBlockMovement = true;
        
        [Header("Replica window data")]
        public List<ReplicaNodeSerialized> Nodes;
        public List<ReplicaEdgeSerialized> Edges;

        [Header("Debug")] 
        [TextArea,SerializeField] private string _textRus;
        
        public bool TryFindStartNode(out ReplicaNodeSerialized node)
        {
            if (Nodes == null || Nodes.Count == 0)
            {
                node = default;
                return false;
            }
            
            return TryFindNode("0", out node);
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

        private void OnValidate()
        {
            if (Nodes != null && Nodes.Count > 0)
            {
                _textRus = "";
                foreach (var node in Nodes)
                {
                    foreach (var localization in node.Localization)
                    {
                        foreach (var part in localization.Parts)
                        {
                            _textRus += part.MessageText;
                        }
                    }
                }
            }
        }
    }
}