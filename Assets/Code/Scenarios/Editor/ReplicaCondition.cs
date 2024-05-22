using System;
using System.Linq;
using Code.Data.Enums;
using Core.Infrastructure.Utils;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Code.Scenarios.Editor
{
    public class ReplicaCondition : VisualElement
    {
        private readonly ReplicaNode node;
        public Port OutputPort;
        public Data.Enums.ReplicaCondition Condition;
        public event Action<ReplicaCondition> OnPressDeleteCondition;

        public ReplicaCondition(ReplicaNode node)
        {
            this.node = node;

            AddPort();
            AddConditions();
            AddDeleteChoiceButton();
        }


        private void AddConditions()
        {
            var condition =
                new PopupField<Data.Enums.ReplicaCondition>(Enum.GetValues(typeof(Data.Enums.ReplicaCondition)).Cast<Data.Enums.ReplicaCondition>().ToList(), 0)
                {
                    label = "Condition",
                    style =
                    {
                        width = 300  // Устанавливаем фиксированную ширину для PopupField
                    }
                };
            condition.RegisterValueChangedCallback(evt =>
            {
                Condition = evt.newValue;
            });
            condition.SetValueWithoutNotify(Condition);
            OutputPort.Add(condition);
        }

        private void AddPort()
        {
            OutputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                typeof(bool));
            OutputPort.portName = "";
            OutputPort.portColor = Color.white;
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