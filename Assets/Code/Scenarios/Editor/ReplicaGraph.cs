using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
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
                "Assets/Code/Scenarios/Styles/ReplicaGraph.uss"
            );

            StyleSheet nodeStyle = (StyleSheet)EditorGUIUtility.Load(
                "Assets/Code/Scenarios/Styles/ReplicaNode.uss"
            );

            styleSheets.Add(graphStyle);
            styleSheets.Add(nodeStyle);
        }

        private void OnPressCreateNode(DropdownMenuAction menuAction)
        {
            var nodePosition = contentViewContainer.WorldToLocal(menuAction.eventInfo.localMousePosition);
            CreateNode(nodePosition);
        }

        public ReplicaNode CreateNode(Vector2 position)
        {
            ReplicaNode node = new ReplicaNode();//todo нужен уникальный айдишник сделать в конструкторе
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