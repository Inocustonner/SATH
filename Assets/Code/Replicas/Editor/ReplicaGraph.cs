using System.Collections.Generic;
using Code.Replicas.Scripts;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Replicas.Editor
{
    public class ReplicaGraph : GraphView
    {
        public ReplicaGraph()
        {
            AddBackground();
            AddStyles();
            AddManipulators();
        }

        private void AddManipulators()
        {
            this.AddManipulator(new SelectionDragger()); //Important! This line should be the first
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(OnMenuEvent));
        }

        private void OnMenuEvent(ContextualMenuPopulateEvent menuEvent)
        {
            menuEvent.menu.AppendAction("Create Node", OnPressCreateNode);
        }

        private void AddBackground()
        {
            GridBackground background = new GridBackground();
            background.StretchToParentSize();
            Insert(0, background);
        }

        private void AddStyles()
        {
            StyleSheet graphStyle = (StyleSheet)EditorGUIUtility.Load(
                "Assets/Code/Replicas/Styles/ReplicaGraph.uss"
            );
            if (graphStyle != null)
            {
                styleSheets.Add(graphStyle);
            }

            StyleSheet nodeStyle = (StyleSheet)EditorGUIUtility.Load(
                "Assets/Code/Replicas/Styles/ReplicaNode.uss"
            );
            if (nodeStyle != null)
            {
                styleSheets.Add(nodeStyle);
            }
        }

            private void OnPressCreateNode(DropdownMenuAction menuAction)
            {
                var nodePosition = contentViewContainer.WorldToLocal(menuAction.eventInfo.localMousePosition);
                CreateNode(nodePosition);
            }

        public ReplicaNode CreateNode(ReplicaNodeSerialized replicaNodeSerialized)
        {
            var conditions = new List<ReplicaConditionElement>();
            for (var index = 0; index < replicaNodeSerialized.Conditions.Count; index++)
            {
                var condition = replicaNodeSerialized.Conditions[index];
                conditions.Add(new ReplicaConditionElement(condition));
            }

            var localizations = new List<LocalizedReplicaElement>();
            for (int i = 0; i < replicaNodeSerialized.Localization.Count; i++)
            {
                var localization = replicaNodeSerialized.Localization[i];
                var parts = new List<ReplicaPartElement>();
                for (int j = 0; j < localization.Parts.Count; j++)
                {
                    var part = localization.Parts[j];
                    parts.Add(new ReplicaPartElement(part.Markups, part.Effect, part.Color, part.MessageText));
                }

                localizations.Add(new LocalizedReplicaElement(localization.Language, parts));
            }

            ReplicaNode node = new ReplicaNode(replicaNodeSerialized.ID, replicaNodeSerialized.TypingSpeed,replicaNodeSerialized.TextTypeAudio ,conditions, localizations);
            node.SetPosition(new Rect(replicaNodeSerialized.EditorPosition, Vector2.zero));
            AddElement(node);
            return node;
        }

        public void FocusOnNode(Vector3 position)
        {
            UpdateViewTransform(-position, Vector3.one);
        }
        public ReplicaNode CreateNode(Vector2 position)
        {
            ReplicaNode node = new ReplicaNode();
            node.SetPosition(new Rect(position, Vector2.zero));
            AddElement(node);
            return node;
        }


        public void CreateEdge(Port inputPort, Port outputPort)
        {
            Edge edge = new Edge
            {
                input = inputPort,
                output = outputPort
            };
            edge.style.color = new StyleColor(Color.white);
            AddElement(edge);
        }

        public void Reset()
        {
            foreach (var edge in edges)
            {
                RemoveElement(edge);
            }

            foreach (var node in nodes)
            {
                RemoveElement(node);
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            foreach (var port in ports)
            {
                if (port == startPort)
                {
                    continue;
                }

                if (port.node == startPort.node)
                {
                    continue;
                }

                if (port.direction == startPort.direction)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }
    }
}