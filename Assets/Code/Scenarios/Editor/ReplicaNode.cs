using System;
using System.Collections.Generic;
using Code.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class ReplicaNode : Node
    {
        public string ID { get; private set; }
        public Port InputPort{ get; private set; }
        public List<ReplicaConditionElement> Conditions { get; private set; } = new();
        public List<LocalizedReplicaElement> Localizations { get; private set; } = new();

        private VisualElement _partsContainer;

        public ReplicaNode()
        {
            AddIdText(0);

            AddConditionButton(1);
            AddConditionElement();
            AddInputPort();
            AddListLocalizedReplicas();
        }
        
        public ReplicaNode(string id, List<ReplicaConditionElement> conditions,List<LocalizedReplicaElement> localizations)
        {
            AddIdText(0, id);

            AddConditionButton(1);
            AddInputPort();
            AddListLocalizedReplicas();

            foreach (var condition in conditions)
            {
                AddConditionElement(condition);
            }

            foreach (var localization in localizations)
            {
                AddLocalizedReplicaElement(localization);
            }
        }
        
        private void AddIdText(int index, string id = "")
        {
            if (id == "")
            {
                id = Guid.NewGuid().ToString();
            }
            var textId = new TextField
            {
                value = "ID"
            };
            textId.AddToClassList("node_id");
            
            textId.RegisterValueChangedCallback(evt => { ID = evt.newValue; });
            textId.SetValueWithoutNotify(ID);
            textId.value = id;
            ID = id;
            
            titleContainer.Insert(index, textId);
        }

        private void AddInputPort()
        {
            InputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "Input Connection";
            InputPort.portColor = Color.white;
            inputContainer.Add(InputPort);
        }

        private  void AddConditionButton(int index)
        {
            var button = new Button(() => AddConditionElement())
            {
                text = "Add Condition"
            };

            mainContainer.Insert(index, button);
        }

        private void AddConditionElement(ReplicaConditionElement condition = null)
        {
            condition ??= new ReplicaConditionElement();
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

        private void AddListLocalizedReplicas()
        {
            _partsContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column
                }
            };
            mainContainer.Add(_partsContainer);

            var addLocalizationButton = new Button(() => AddLocalizedReplicaElement())
            {
                text = "Add Localization",
                style =
                {
                    backgroundColor = Constance.PurpleColor,
                    color = Color.white
                }
            };
            mainContainer.Add(addLocalizationButton);
        }

        private void AddLocalizedReplicaElement(LocalizedReplicaElement localization = null)
        {
            localization ??= new LocalizedReplicaElement();
            localization.OnPressDeleteLocalization += element =>
            {
                Localizations.Remove(element);
                _partsContainer.Remove(element);
            };
            Localizations.Add(localization);
            _partsContainer.Add(localization);
        }
    }
}