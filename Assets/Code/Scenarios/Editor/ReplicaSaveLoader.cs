using System.Collections.Generic;
using System.Linq;
using Code.Scenarios.Scripts;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Code.Scenarios.Editor
{
    public sealed class ReplicaSaveLoader
    {
        public static void LoadReplica(ReplicaGraph graphView, ReplicaConfig config)
        {
            if (config == null)
            {
                Debug.LogWarning("Replica config is null!");
                return;
            }

            var nodes = new List<ReplicaNode>();
            foreach (var serializedNode in config.nodes)
            {
                var node = graphView.CreateNode(serializedNode.EditorPosition);
                node.ID = serializedNode.ID;
               //node.MessageText = serializedNode.Message;

                foreach (var serializedChoice in serializedNode.Conditions)
                {
                    node.CreateCondition(serializedChoice);
                }

                nodes.Add(node);
            }

            foreach (var serializedEdge in config.edges)
            {
                var outputId = serializedEdge.SourceNode;
                var outputNode = nodes.First(it => it.ID == outputId);
                var outputPort = outputNode.Conditions[serializedEdge.Index].OutputPort;

                var inputId = serializedEdge.TargetNode;
                var inputNode = nodes.First(it => it.ID == inputId);
                var inputPort = inputNode.InputPort;

                graphView.CreateEdge(inputPort, outputPort);
            }
        }

        public static void CreateReplica(ReplicaGraph graph, out ReplicaConfig config)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save file", "Dialog", "asset", "");
            config = ScriptableObject.CreateInstance<ReplicaConfig>();
            
            SaveReplica(graph, config);
            
            AssetDatabase.CreateAsset(config, path);
            AssetDatabase.SaveAssets();
        }

        public static void SaveReplica(ReplicaGraph graph, ReplicaConfig config)
        {
            config.nodes = ConvertNodesData(graph);
            config.edges = ConvertEdgesToData(graph);
            EditorUtility.SetDirty(config);
        }
        
        private static List<ReplicaNodeSerialized> ConvertNodesData(GraphView graphView)
        {
            var result = new List<ReplicaNodeSerialized>();
            
            foreach (var node in graphView.nodes)
            {
                var dialogueNode = (ReplicaNode) node;
                var serializedNode = new ReplicaNodeSerialized
                {
                    ID = dialogueNode.ID,
                    //Message = dialogueNode.MessageText,
                    EditorPosition = dialogueNode.GetPosition().center,
                    Conditions = ConvertConditionsToData(dialogueNode)
                };

                result.Add(serializedNode);
            }

            return result;
        }

        private static List<Data.Enums.ReplicaCondition> ConvertConditionsToData(ReplicaNode nodeView)
        {
            return nodeView.Conditions.Select(choice => choice.Condition).ToList();
        }

        private static List<ReplicaEdgeSerialized> ConvertEdgesToData(GraphView graphView)
        {
            var serializedEdges = new List<ReplicaEdgeSerialized>();
            
            foreach (var edge in graphView.edges)
            {
                var outputPort = edge.output;
                var inputPort = edge.input;
                
                var outputNode = (ReplicaNode) outputPort.node;
                var inputNode = (ReplicaNode) inputPort.node;

                var serializedEdge = new ReplicaEdgeSerialized
                {
                    SourceNode = outputNode.ID,
                    TargetNode = inputNode.ID,
                    Index = outputNode.Conditions.FindIndex(it => it.OutputPort == outputPort)
                };

                serializedEdges.Add(serializedEdge);
            }

            return serializedEdges;
        }
    }
}