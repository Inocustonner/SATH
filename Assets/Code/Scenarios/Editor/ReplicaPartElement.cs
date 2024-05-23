using System;
using System.Linq;
using Code.Data.Enums;
using Code.Utils;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public sealed class ReplicaPartElement : VisualElement
    {
        public TextMarkup Markup { get; private set; }
        public TextEffect Effect { get; private set; }
        public Color Color { get; private set; }
        public string MessageText { get; private set; }
        public event Action<ReplicaPartElement> OnPressDeletePart; 
        
        public ReplicaPartElement()
        {
            style.backgroundColor = new StyleColor(Color.white);
            style.minWidth = 100;
            style.minHeight = 20;

            AddMarkupProperty(0);
            AddEffectProperty(1);
            AddColorProperty(2);
            AddMessageTextField(3);
            AddDeletePartButton(4);
        }

        public ReplicaPartElement(TextMarkup markup, TextEffect effect, Color color, string messageText)
        {
            style.backgroundColor = new StyleColor(Color.white);
            style.minWidth = 100;
            style.minHeight = 20;

            AddMarkupProperty(0, markup);
            AddEffectProperty(1, effect);
            AddColorProperty(2, color);
            AddMessageTextField(3, messageText);
            AddDeletePartButton(4);
        }

        private void AddMarkupProperty(int index, TextMarkup markup = TextMarkup.Default)
        {
            var property = new PopupField<TextMarkup>(Enum.GetValues(typeof(TextMarkup)).Cast<TextMarkup>().ToList(), 0)
            {
                label = "Markup"
            };
            property.labelElement.style.color = new StyleColor(Constance.PurpleColor);
            
            property.RegisterValueChangedCallback(evt => { Markup = evt.newValue; });
            property.SetValueWithoutNotify(Markup);
            property.value = markup;
            Markup = markup;
            
            Insert(index, property);
        }

        private void AddEffectProperty(int index, TextEffect effect = TextEffect.Default)
        {
            var property = new PopupField<TextEffect>(Enum.GetValues(typeof(TextEffect)).Cast<TextEffect>().ToList(), 0)
            {
                label = "Effect",
            };
            property.labelElement.style.color = new StyleColor(Constance.PurpleColor);

            property.RegisterValueChangedCallback(evt => { Effect = evt.newValue; });
            property.SetValueWithoutNotify(Effect);
            property.value = effect;
            Effect = effect;
            
            Insert(index, property);
        }

        private void AddColorProperty(int index, Color color = default)
        {
            var property = new ColorField()
            {
                label = "Color"
            };
            property.labelElement.style.color = new StyleColor(Constance.PurpleColor);

            property.UnregisterValueChangedCallback(evt => { Color = evt.newValue; });
            property.SetValueWithoutNotify(Color);
            property.value = color;
            Color = color;
            
            Insert(index, property);
        }

        private void AddMessageTextField(int index, string text = "")
        {
            var textField = new TextField()
            {
                multiline = true,
                value = "Message",
            };
            textField.labelElement.style.color = new StyleColor(Constance.PurpleColor);
            textField.AddToClassList("node_message");

            textField.RegisterValueChangedCallback(evt => { MessageText = evt.newValue; });
            textField.SetValueWithoutNotify(MessageText);
            textField.value = text;
            MessageText = text;
            
            Insert(index, textField);
        }

        private void AddDeletePartButton(int index)
        {
            var addPartButton = new Button(() => OnPressDeletePart?.Invoke(this))
            {
                text = "x",
                style =
                {
                    color = new StyleColor(Color.white),
                    backgroundColor = new StyleColor(Color.gray),
                    maxWidth = 15,
                    maxHeight = 15,
                }
            };
            Insert(index, addPartButton);
        }
    }
}