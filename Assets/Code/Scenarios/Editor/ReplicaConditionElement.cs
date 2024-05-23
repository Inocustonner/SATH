using System;
using System.Linq;
using Code.Data.Enums;
using Code.Utils;
using Core.Infrastructure.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Code.Scenarios.Editor
{
    public class ReplicaConditionElement : VisualElement
    {
        public ReplicaCondition Condition { get; private set; }
        public Port OutputPort { get; private set; }
        public event Action<ReplicaConditionElement> OnPressDeleteCondition;

        public ReplicaConditionElement()
        {
            AddPort();
            AddConditions();
            AddDeleteChoiceButton();
        }

        public ReplicaConditionElement(ReplicaCondition condition)
        {
            AddPort();
            AddConditions(condition);
            AddDeleteChoiceButton();
            
            Condition = condition;
        }

        private void AddConditions()
        {
            var condition =
                new PopupField<ReplicaCondition>(Enum.GetValues(typeof(ReplicaCondition)).Cast<ReplicaCondition>().ToList(), 0)
                {
                    label = "Condition",
                    style =
                    {
                        width = 300
                    }
                };
            condition.RegisterValueChangedCallback(evt =>
            {
                Condition = evt.newValue;
            });
            condition.SetValueWithoutNotify(Condition);
            OutputPort.Add(condition);
        }

        private void AddConditions(ReplicaCondition replicaCondition)
        {
            var condition =
                new PopupField<ReplicaCondition>(Enum.GetValues(typeof(ReplicaCondition)).Cast<ReplicaCondition>().ToList(), 0)
                {
                    label = "Condition",
                    style =
                    {
                        width = 300
                    }
                };
            condition.RegisterValueChangedCallback(evt =>
            {
                Condition = evt.newValue;
            });
            condition.SetValueWithoutNotify(Condition);
            condition.value = replicaCondition;
            OutputPort.Add(condition);
        }

        
        private void AddPort()
        {
            OutputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                typeof(bool));
            OutputPort.portName = "";
            OutputPort.portColor = Constance.PurpleColor;
            Add(OutputPort);
        }

        private void AddDeleteChoiceButton()
        {
            var button = new Button(() =>
            {
                this.Log("Press delete condition");

                OnPressDeleteCondition?.Invoke(this);
            })
            {
                text = "x"
            };

            OutputPort.Add(button);
        }
    }
}