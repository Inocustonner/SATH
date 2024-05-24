using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Code.Data.Configs;
using Code.Data.DynamicData;
using Code.Data.Enums;
using Code.Infrastructure.Services;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Scenarios.Scripts
{
    public sealed class ReplicaConverter
    {
        private ReplicaConfig _config;
        private ReplicaNodeSerialized _currentNode;
        private readonly GameConditionService _gameConditionService;

        public ReplicaConverter(GameConditionService gameConditionService)
        {
            _gameConditionService = gameConditionService;
        }
        
        
        public void SetConfig(ReplicaConfig config)
        {
            if (!config.TryFindStartNode(out var node))
            {
                this.LogError("Entry point is absent!");
            }

            _config = config;
            _currentNode = node;
        }

        public bool TryGetAcceleratedTexts(Lan language, out AcceleratedText[] texts)
        {
            var list = new List<AcceleratedText>();
            for (int i = 0; i < _config.Nodes.Count; i++)
            {
                if (TryGetAcceleratedText(language, out var text))
                {
                    list.Add(text);

                    int conditionIndex;
                    for (conditionIndex = 0; conditionIndex < _currentNode.Conditions.Count; conditionIndex++)
                    {
                        if (_gameConditionService.GetValue(_currentNode.Conditions[conditionIndex]))
                        {
                            break;
                        }
                    }

                    if (!MoveNext(conditionIndex))
                    {
                        break;
                    }
                }
            }

            texts = list.ToArray();
            return texts.Length > 0;
        }

        private bool TryGetAcceleratedText(Lan language, out AcceleratedText replicas)
        {
            replicas = new AcceleratedText();
            var text = "";
            var localization = _currentNode.Localization.FirstOrDefault(l => l.Language == language);
                    Debug.Log($"Добавлена реплика в список {localization.Parts != null}  {localization.Parts.Count}");
            if (localization.Parts != null && localization.Parts.Count > 0)
            {
                foreach (var part in localization.Parts)
                {
                    text += GetFormatReplica(part) + " ";
                }

                replicas.Text = text;
                replicas.Speed = _currentNode.TypingSpeed;
                return true;
            }

            return false;
        }

        private bool MoveNext(int conditionIndex)
        {
            if (_config.TryFindNextNode(_currentNode.ID, conditionIndex, out var nextNode))
            {
                _currentNode = nextNode;
                this.Log($"реплика смогла перейти на следующую часть {_currentNode.ID}");
                return true;
            }

            return false;
        }

        private string GetFormatReplica(ReplicaPartSerialized part)
        {
            StringBuilder formattedText = new StringBuilder();

            string text = part.MessageText;

            if (part.Color != new Color() && part.Color != Color.white && part.Color.a != 0)
            {
                string hexCode = ColorUtility.ToHtmlStringRGBA(part.Color);

                text = $"<color=#{hexCode}>{text}</color>";
            }

            if (part.Markup != TextMarkup.Default)
            {
                text = part.Markup switch
                {
                    TextMarkup.Bold => $"<b>{text}</b>",
                    TextMarkup.Italic => $"<i>{text}</i>",
                    TextMarkup.Underline => $"<u>{text}</u>",
                    TextMarkup.Strikethrough => $"<s>{text}</s>",
                    _ => text
                };
            }

            if (part.Effect != TextEffect.Default)
            {
                text = $"<{part.Effect.ToString().ToLower()}>{text}</{part.Effect.ToString().ToLower()}>";
            }

            formattedText.Append(text);

            return formattedText.ToString();
        }
    }
}