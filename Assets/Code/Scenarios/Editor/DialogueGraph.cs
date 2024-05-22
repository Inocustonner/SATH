using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class DialogueGraph : GraphView
    {
        public DialogueGraph()
        {
            AddBackground();
            AddStyles();
            AddManipulators();
            CreateNode(Vector2.zero);
        }

        private void AddManipulators()
        {
            this.AddManipulator(new SelectionDragger());//only first
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(OnMenuEvent));
        }

        private void OnMenuEvent(ContextualMenuPopulateEvent menuEvent)
        {
            menuEvent.menu.AppendAction("Create Node", OnCreateNode);
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
                "Assets/Code/Scenarios/Styles/DialogueGraph.uss"
            );

            StyleSheet nodeStyle = (StyleSheet)EditorGUIUtility.Load(
                "Assets/Code/Scenarios/Styles/DialogueNode.uss"
            );

            styleSheets.Add(graphStyle);
          styleSheets.Add(nodeStyle);
        }

        private void OnCreateNode(DropdownMenuAction menuAction)
        {
            var nodePosition = menuAction.eventInfo.localMousePosition;
            CreateNode(nodePosition);
        }

        public DialogueNode CreateNode(Vector2 position)
        {
            DialogueNode node = new DialogueNode();
            node.SetPosition(new Rect(position, Vector2.zero));
            AddElement(node);
            return node;
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
    }
}