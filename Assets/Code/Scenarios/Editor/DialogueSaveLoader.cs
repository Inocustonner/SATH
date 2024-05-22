using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Scenarios.Editor;
using Code.Scenarios.Scripts;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lessons.MetaGame.Dialogs
{
    public sealed class DialogueSaveLoader
    {
        public static void LoadDialog(DialogueGraph graphView, DialogueConfig config)
        {
            if (config == null)
            {
                Debug.LogWarning("Dialog is null!");
                return;
            }

            var nodes = new List<DialogueNode>();
            foreach (var serializedNode in config.nodes)
            {
                var node = graphView.CreateNode(serializedNode.EditorPosition);
                node.ID = serializedNode.ID;
                node.MessageText = serializedNode.Message;

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

        public static void CreateDialog(DialogueGraph graph, out DialogueConfig config)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save file", "Dialog", "asset", "");
            config = ScriptableObject.CreateInstance<DialogueConfig>();
            
            SaveDialog(graph, config);
            
            AssetDatabase.CreateAsset(config, path);
            AssetDatabase.SaveAssets();
        }

        public static void SaveDialog(DialogueGraph graph, DialogueConfig config)
        {
            config.nodes = ConvertNodesData(graph);
            config.edges = ConvertEdgesToData(graph);
            EditorUtility.SetDirty(config);
        }
        
        private static List<DialogueNodeSerialized> ConvertNodesData(GraphView graphView)
        {
            var result = new List<DialogueNodeSerialized>();
            
            foreach (var node in graphView.nodes)
            {
                var dialogueNode = (DialogueNode) node;
                var serializedNode = new DialogueNodeSerialized
                {
                    ID = dialogueNode.ID,
                    Message = dialogueNode.MessageText,
                    EditorPosition = dialogueNode.GetPosition().center,
                    Conditions = ConvertChoicesToData(dialogueNode)
                };

                result.Add(serializedNode);
            }

            return result;
        }

        private static List<TextCondition> ConvertChoicesToData(DialogueNode nodeView)
        {
            var serializedChoices = new List<TextCondition>();
            foreach (var choice in nodeView.Conditions)
            {
                var serializedChoice = choice.Condition;
                serializedChoices.Add(serializedChoice);
            }
            
            return serializedChoices;
        }

        private static List<DialogueEdgeSerialized> ConvertEdgesToData(GraphView graphView)
        {
            var serializedEdges = new List<DialogueEdgeSerialized>();
            
            foreach (var edge in graphView.edges)
            {
                var outputPort = edge.output;
                var inputPort = edge.input;
                
                var outputNode = (DialogueNode) outputPort.node;
                var inputNode = (DialogueNode) inputPort.node;

                var serializedEdge = new DialogueEdgeSerialized
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