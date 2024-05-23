using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.Configs;
using Code.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class LocalizedReplicaElement : VisualElement
    {
        public Lan Language;
        public List<ReplicaPartElement> Parts = new();
        private VisualElement _partsContainer;
        
        public LocalizedReplicaElement()
        {
            AddLanguageProperty();
            AddListParts();
        }

        public LocalizedReplicaElement(Lan language, List<ReplicaPartElement> parts)
        {
            AddLanguageProperty(language);
            AddListParts();
            
            Language = language;
            foreach (var part in parts)
            {
                AddReplicaPart(part);
            }
        }
        
        private void AddLanguageProperty(Lan language = Lan.Rus)
        {
            var property =
                new PopupField<Lan>(Enum.GetValues(typeof(Lan)).Cast<Lan>().ToList(), 0)
                {
                    label = "Language",
                    style =
                    {
                        width = 300,
                        backgroundColor = Constance.PurpleColor
                    }
                };
            property.RegisterValueChangedCallback(evt =>
            {
                Language = evt.newValue;
            });
            property.value = language;
            property.SetValueWithoutNotify(Language);
            Add(property);
        }

        private void AddListParts()
        {
            _partsContainer = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column
                }
            };
            Add(_partsContainer);

            var addPartButton = new Button(AddReplicaPart) { text = "Add Part" };
            addPartButton.style.color = new StyleColor(Constance.PurpleColor);
            addPartButton.style.backgroundColor = new StyleColor(Color.white);
            Add(addPartButton);
        }

        private void AddReplicaPart()
        {
            var part = new ReplicaPartElement();
            Parts.Add(part);
            _partsContainer.Add(part);
        }

        private void AddReplicaPart(ReplicaPartElement part)
        {
            Parts.Add(part);
            _partsContainer.Add(part);
        }
    }
}

