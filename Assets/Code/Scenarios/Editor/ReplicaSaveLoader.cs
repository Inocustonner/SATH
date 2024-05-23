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
            foreach (var serializedNode in config.Nodes)
            {
                var node = graphView.CreateNode(serializedNode);
                nodes.Add(node);
            }

            foreach (var serializedEdge in config.Edges)
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
            var path = EditorUtility.SaveFilePanelInProject("Save file", "Replica_", "asset", "");
            config = ScriptableObject.CreateInstance<ReplicaConfig>();
            
            SaveReplica(graph, config);
            
            AssetDatabase.CreateAsset(config, path);
            AssetDatabase.SaveAssets();
        }

        public static void SaveReplica(ReplicaGraph graph, ReplicaConfig config)
        {
            config.Nodes = ConvertNodesData(graph);
            config.Edges = ConvertEdgesToData(graph);
            EditorUtility.SetDirty(config);
        }
        
        private static List<ReplicaNodeSerialized> ConvertNodesData(GraphView graphView)
        {
            var result = new List<ReplicaNodeSerialized>();
            
            foreach (var node in graphView.nodes)
            {
                var replicaNode = (ReplicaNode) node;

                var localizationsSerialized = new List<LocalizationSerialized>();
                foreach (var localization in replicaNode.Localizations)
                {
                    var partsSerialized = new List<ReplicaPartSerialized>();
                    foreach (var part in localization.Parts)
                    {
                        partsSerialized.Add(new ReplicaPartSerialized()
                        {
                            Color = part.Color,
                            Effect = part.Effect,
                            Markup = part.Markup,
                            MessageText = part.MessageText
                        });
                    }
                    
                    localizationsSerialized.Add(new LocalizationSerialized()
                    {
                        Language = localization.Language,
                        Parts = partsSerialized
                    });
                }

                var serializedNode = new ReplicaNodeSerialized
                {
                    ID = replicaNode.ID,
                    Localization = localizationsSerialized,
                    EditorPosition = replicaNode.GetPosition().center,
                    Conditions = ConvertConditionsToData(replicaNode)
                };

                result.Add(serializedNode);
            }

            return result;
        }

        private static List<Data.Enums.GameCondition> ConvertConditionsToData(ReplicaNode nodeView)
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