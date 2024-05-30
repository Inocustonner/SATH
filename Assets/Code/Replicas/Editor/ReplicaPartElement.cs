using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Utils;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Replicas.Editor
{
    public sealed class ReplicaPartElement : VisualElement
    {
        public List<TextMarkup> Markups { get; private set; } = new List<TextMarkup>();
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

        public ReplicaPartElement(List<TextMarkup> markups, TextEffect effect, Color color, string messageText)
        {
            style.backgroundColor = new StyleColor(Color.white);
            style.minWidth = 100;
            style.minHeight = 20;

            AddMarkupProperty(0, markups);
            AddEffectProperty(1, effect);
            AddColorProperty(2, color);
            AddMessageTextField(3, messageText);
            AddDeletePartButton(4);
        }

        private void AddMarkupProperty(int index, List<TextMarkup> markups = null)
        {
            markups = markups ?? new List<TextMarkup>();
            Markups = markups;

            var container = new VisualElement();
            var label = new Label("Markups")
            {
                style =
                {
                    color = new StyleColor(Constance.PurpleColor)
                }
            };
            container.Add(label);

            var horizontalContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.Wrap
                }
            };

            var availableMarkups = Enum.GetValues(typeof(TextMarkup)).Cast<TextMarkup>().ToList();
            for (var i = 1; i < availableMarkups.Count; i++)
            {
                var markup = availableMarkups[i];
                var toggle = new Toggle(markup.ToString())
                {
                    value = Markups.Contains(markup),
                    style = { backgroundColor = Constance.DarkPurpleColor}
                };
                toggle.RegisterValueChangedCallback(evt =>
                {
                    if (evt.newValue)
                    {
                        if (!Markups.Contains(markup))
                        {
                            Markups.Add(markup);
                        }
                    }
                    else
                    {
                        if (Markups.Contains(markup))
                        {
                            Markups.Remove(markup);
                        }
                    }
                });
                horizontalContainer.Add(toggle);
            }

            container.Add(horizontalContainer);
            Insert(index, container);
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

            property.RegisterValueChangedCallback(evt => { Color = evt.newValue; });
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
