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
        public Lan Language { get; private set; }
        public List<ReplicaPartElement> Parts { get; private set; } = new();
        
        private VisualElement _partsContainer;
        public event Action<LocalizedReplicaElement> OnPressDeleteLocalization;
        
        public LocalizedReplicaElement()
        {
            AddLanguageProperty(0);
            AddDeleteButton(1);
            AddListParts();
        }

        public LocalizedReplicaElement(Lan language, List<ReplicaPartElement> parts)
        {
            AddLanguageProperty(0,language);
            AddDeleteButton(1);
            AddListParts();
            
            Language = language;
            foreach (var part in parts)
            {
                AddReplicaPart(part);
            }
        }
        
        private void AddLanguageProperty(int index,Lan language = Lan.Rus)
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
            Insert(index, property);
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

            var addPartButton = new Button(AddReplicaPart)
            {
                text = "Add Replica Part",
                style =
                {
                    color = new StyleColor(Constance.PurpleColor),
                    backgroundColor = new StyleColor(Color.white)
                }
            };
            Add(addPartButton);
        }

        private void AddReplicaPart()
        {
            var part = new ReplicaPartElement();
            part.OnPressDeletePart += element =>
            {
                Parts.Remove(element);
                _partsContainer.Remove(element);
            }; 
            Parts.Add(part);
            _partsContainer.Add(part);
        }

        private void AddReplicaPart(ReplicaPartElement part)
        {
            Parts.Add(part);
            _partsContainer.Add(part);
        }
        
        private void AddDeleteButton(int index)
        {
            var addPartButton = new Button(() => OnPressDeleteLocalization?.Invoke(this))
            {
                text = "Delete Localization",
                style =
                {
                    color = new StyleColor(Color.white),
                    backgroundColor = new StyleColor(Constance.DarkPurpleColor),
                    maxWidth = 120,
                    minHeight = 10
                }
            };
            Insert(index, addPartButton);
        }
    }
}

