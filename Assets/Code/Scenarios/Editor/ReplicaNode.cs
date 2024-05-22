using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class ReplicaNode : Node
    {
        public TextMarkup Markup;
        public TextEffect Effect;
        public string MessageText;
        public string ID;

        public List<ReplicaCondition> Conditions = new();
        public Port InputPort;
        public ReplicaNode()
        {
            AddIdText(0);
            AddMessageText(1);
            AddMarkup(2);
            AddConditionButton(3);
            AddCondition();
            AddInputPort();
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
            var condition = new ReplicaCondition(this);
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
            textId.RegisterValueChangedCallback(evt =>
            {
                ID = evt.newValue;
            });
            textId.SetValueWithoutNotify(ID);
            titleContainer.Insert(index, textId);
        }

        private void AddMessageText(int index)
        {
            var messageText = new TextField()
            {
                multiline = true,
                value = "Message"
            };
            messageText.AddToClassList("node_message");
            messageText.RegisterValueChangedCallback(evt =>
            {
                MessageText = evt.newValue;
            });
            messageText.SetValueWithoutNotify(MessageText);
            mainContainer.Insert(index,messageText);
        }

        private void AddInputPort()
        {
            InputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "Input Connection";
            InputPort.portColor = Color.white;
            inputContainer.Add(InputPort);
        }
        
        private void AddMarkup(int index)
        {
            var markup = new PopupField<TextMarkup>(Enum.GetValues(typeof(TextMarkup)).Cast<TextMarkup>().ToList(), 0)
            {
                label = "Markup"
            };
            markup.RegisterValueChangedCallback(evt =>
            {
                Markup = evt.newValue;
            });
            markup.SetValueWithoutNotify(Markup);
            mainContainer.Insert(index ,markup);
        }

        public void CreateCondition(Data.Enums.ReplicaCondition replicaCondition)
        {
            var condition = new ReplicaCondition(this);
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