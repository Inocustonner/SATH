using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Replicas.Editor
{
    public class ReplicaNode : Node
    {
        public string ID { get; private set; }
        public float TypingSpeed { get; private set; }
        public TextTypingAudioType TextTypeAudio { get; private set; }
        public Port InputPort{ get; private set; }
        public List<ReplicaConditionElement> Conditions { get; private set; } = new();
        public List<LocalizedReplicaElement> Localizations { get; private set; } = new();

        private VisualElement _partsContainer;

        public ReplicaNode()
        {
            AddIdText(0);
            AddTypingSpeedProperty(1);
            AddTextTypeAudio(2);
            AddConditionButton(3);
            AddConditionElement();
            AddInputPort();
            AddListLocalizedReplicas();
        }

        public ReplicaNode(string id, float typingSpeed, TextTypingAudioType textTypingAudioType,
            List<ReplicaConditionElement> conditions, List<LocalizedReplicaElement> localizations)
        {
            AddIdText(0, id);
            AddTypingSpeedProperty(1, typingSpeed);
            AddTextTypeAudio(2,textTypingAudioType);
            AddConditionButton(3);
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

        private void AddTextTypeAudio(int index, TextTypingAudioType textTypeAudio = TextTypingAudioType.Default)
        {
            var property = new PopupField<TextTypingAudioType>(Enum.GetValues(typeof(TextEffect)).Cast<TextTypingAudioType>().ToList(), 0)
            {
                label = "TextTypeAudio",
                style =
                {
                    width = 300,
                }
            };
            property.labelElement.style.color = new StyleColor(Color.white);

            property.RegisterValueChangedCallback(evt => { TextTypeAudio = evt.newValue; });
            property.SetValueWithoutNotify(textTypeAudio);
            property.value = textTypeAudio;
            TextTypeAudio = textTypeAudio;
            
            mainContainer.Insert(index, property);
        }

        private void AddIdText(int index, string id = "")
        {
            if (id == "")
            {
                id = Guid.NewGuid().ToString();
            }
            var textId = new TextField();
            textId.AddToClassList("node_id");
            
            textId.RegisterValueChangedCallback(evt => { ID = evt.newValue; });
            textId.SetValueWithoutNotify(ID);
            textId.value = id;
            ID = id;
            
            titleContainer.Insert(index, textId);
        }

        private void AddTypingSpeedProperty(int index, float typingSpeed = 0)
        {
            var property = new FloatField("Typing Speed");
            property.RegisterValueChangedCallback(evt => { TypingSpeed = evt.newValue; });
            property.SetValueWithoutNotify(TypingSpeed);
            property.value = typingSpeed;
            TypingSpeed = typingSpeed;
            
            mainContainer.Insert(index, property);
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