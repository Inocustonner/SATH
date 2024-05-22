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
    public class DialogueCondition : VisualElement
    {
        private readonly DialogueNode node;
        public Port OutputPort;
        public TextCondition Condition;
        public event Action<DialogueCondition> OnPressDeleteCondition;

        public DialogueCondition(DialogueNode node)
        {
            this.node = node;

            AddPort();
            AddConditions();
            AddDeleteChoiceButton();
        }


        private void AddConditions()
        {
            var condition =
                new PopupField<TextCondition>(Enum.GetValues(typeof(TextCondition)).Cast<TextCondition>().ToList(), 0)
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