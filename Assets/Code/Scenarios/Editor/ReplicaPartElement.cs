using System;
using System.Linq;
using Code.Data.Enums;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class ReplicaPartElement: VisualElement
    {
        public TextMarkup Markup;
        public TextEffect Effect;
        public Color Color; 
        public string MessageText;

        private readonly Color _propertyColor = new Color(0.4941177f, 0.1411765f, 0.4941177f);
        public ReplicaPartElement()
        {
            style.backgroundColor = new StyleColor(Color.white);
            style.minWidth = 100; 
            style.minHeight = 20; 

            AddMarkupProperty(0);
            AddEffectProperty(1);
            AddColorProperty(2);
            AddMessageText(3);
        }
        
        private void AddMarkupProperty(int index)
        {
            var property = new PopupField<TextMarkup>(Enum.GetValues(typeof(TextMarkup)).Cast<TextMarkup>().ToList(), 0)
            {
                label = "Markup"
            };
            property.labelElement.style.color = new StyleColor(_propertyColor);
        
            property.RegisterValueChangedCallback(evt =>
            {
                Markup = evt.newValue;
            });
            property.SetValueWithoutNotify(Markup);
           
            Insert(index,property);
        }

        private void AddEffectProperty(int index)
        {
            var property = new PopupField<TextEffect>(Enum.GetValues(typeof(TextEffect)).Cast<TextEffect>().ToList(), 0)
            {
                label = "Effect",
            };
            property.labelElement.style.color = new StyleColor(_propertyColor);
        
            property.RegisterValueChangedCallback(evt =>
            {
                Effect = evt.newValue;
            });
            property.SetValueWithoutNotify(Effect);
         
            Insert(index,property);
        }

        private void AddColorProperty(int index)
        {
            var property = new ColorField()
            {
                label = "Color"
            };
            property.labelElement.style.color = new StyleColor(_propertyColor);

            property.UnregisterValueChangedCallback(evt =>
            {
                Color = evt.newValue;
            });
            property.SetValueWithoutNotify(Color);
            
            Insert(index,property);
        }
        private void AddMessageText(int index)
        {
            var messageText = new TextField()
            {
                multiline = true,
                value = "Message",
            };
            messageText.labelElement.style.color = new StyleColor(_propertyColor);
            messageText.AddToClassList("node_message");
          
            messageText.RegisterValueChangedCallback(evt =>
            {
                MessageText = evt.newValue;
            });
            messageText.SetValueWithoutNotify(MessageText);
            
            Insert(index,messageText);
        }
    }
}