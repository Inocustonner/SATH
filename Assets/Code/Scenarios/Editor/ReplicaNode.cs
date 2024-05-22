using System.Collections.Generic;
using Code.Data.Enums;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class ReplicaNode : Node
    {
        public string ID;
        public Port InputPort;
        public List<ReplicaConditionElement> Conditions = new();
        public List<ReplicaPartElement> Parts = new();

        private VisualElement _partsContainer;

        public ReplicaNode()
        {
            AddIdText(0);

            AddConditionButton(1);
            AddCondition();
            AddInputPort();
            AddListParts();
        }

        private void AddListParts()
        {
            _partsContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column
                }
            };
            mainContainer.Add(_partsContainer);

            var addPartButton = new Button(AddReplicaPart) { text = "Add Part" };
            mainContainer.Add(addPartButton);
        }

        private void AddReplicaPart()
        {
            var part = new ReplicaPartElement();
            Parts.Add(part);
            _partsContainer.Add(part);
        }

        private void AddConditionButton(int index)
        {
            var button = new Button(AddCondition)
            {
                text = "Add Condition"
            };

            mainContainer.Insert(index, button);
        }

        private void AddCondition()
        {
            var condition = new ReplicaConditionElement(this);
            Conditions.Add(condition);

            condition.OnPressDeleteCondition += dialogueCondition =>
            {
                if (Conditions.Count > 1)
                {
                    Conditions.Remove(condition);
                    outputContainer.Remove(dialogueCondition);
                }
            };

            outputContainer.Add(condition);
        }

        private void AddIdText(int index)
        {
            var textId = new TextField
            {
                value = "ID"
            };
            textId.AddToClassList("node_id");
            textId.RegisterValueChangedCallback(evt => { ID = evt.newValue; });
            textId.SetValueWithoutNotify(ID);
            titleContainer.Insert(index, textId);
        }

        private void AddInputPort()
        {
            InputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "Input Connection";
            InputPort.portColor = Color.white;
            inputContainer.Add(InputPort);
        }

        public void CreateCondition(ReplicaCondition replicaCondition)
        {
            var condition = new ReplicaConditionElement(this);
            Conditions.Add(condition);

            condition.OnPressDeleteCondition += dialogueCondition =>
            {
                if (Conditions.Count > 1)
                {
                    Conditions.Remove(condition);
                    outputContainer.Remove(dialogueCondition);
                }
            };

            outputContainer.Add(condition);
        }
    }
}