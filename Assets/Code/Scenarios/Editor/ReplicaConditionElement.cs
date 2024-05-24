using System;
using System.Linq;
using Code.Data.Enums;
using Code.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Code.Scenarios.Editor
{
    public class ReplicaConditionElement : VisualElement
    {
        public GameCondition Condition { get; private set; }
        public Port OutputPort { get; private set; }
        public event Action<ReplicaConditionElement> OnPressDeleteCondition;

        public ReplicaConditionElement()
        {
            AddPort();
            AddConditions();
            AddDeleteChoiceButton();
        }

        public ReplicaConditionElement(GameCondition condition)
        {
            AddPort();
            AddConditions(condition);
            AddDeleteChoiceButton();
            
            Condition = condition;
        }

        private void AddConditions()
        {
            var condition =
                new PopupField<GameCondition>(Enum.GetValues(typeof(GameCondition)).Cast<GameCondition>().ToList(), 0)
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

        private void AddConditions(GameCondition gameCondition)
        {
            var condition =
                new PopupField<GameCondition>(Enum.GetValues(typeof(GameCondition)).Cast<GameCondition>().ToList(), 0)
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
            condition.value = gameCondition;
            OutputPort.Add(condition);
        }

        
        private void AddPort()
        {
            OutputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            OutputPort.portName = "";
            OutputPort.portColor = Constance.PurpleColor;
            Add(OutputPort);
        }

        private void AddDeleteChoiceButton()
        {
            var button = new Button(() =>
            {
                OnPressDeleteCondition?.Invoke(this);
            })
            {
                text = "x"
            };

            OutputPort.Add(button);
        }
    }
}