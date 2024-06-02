using System.Collections.Generic;
using System.Linq;
using System.Text;
using Code.Data.Configs;
using Code.Data.DynamicData;
using Code.Data.Enums;
using Code.Infrastructure.Services;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Replicas.Scripts
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
                this.LogError($"Could not find replica with ID [0] in the config {config.name}");
            }

            _config = config;
            _currentNode = node;
        }

        public bool TryGetAcceleratedTexts(Lan language, out AcceleratedTextData[] texts)
        {
            var list = new List<AcceleratedTextData>();
            for (int i = 0; i < _config.Nodes.Count; i++)
            {
                if (TryGetAcceleratedText(language, out var text))
                {
                    if (text.Text is not "" or " ")
                    {
                        list.Add(text);
                    }
                }
                
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

            texts = list.ToArray();
            return texts.Length > 0;
        }

        private bool TryGetAcceleratedText(Lan language, out AcceleratedTextData replicas)
        {
            replicas = new AcceleratedTextData();
            var text = "";
            var localization = _currentNode.Localization.FirstOrDefault(l => l.Language == language);
            if (localization.Parts != null && localization.Parts.Count > 0)
            {
                foreach (var part in localization.Parts)
                {
                    var formatReplica = GetFormatReplica(part);
                    if (formatReplica[^1] != ' ')
                    {
                        formatReplica += " ";
                    }
                    text += formatReplica;
                }

                replicas.Text = text;
                replicas.Speed = _currentNode.TypingSpeed;
                return true;
            }

            return false;
        }
        
        private AcceleratedTextData GetAcceleratedText(Lan language)
        {
            var replicas = new AcceleratedTextData();
            var text = "";
            var localization = _currentNode.Localization.FirstOrDefault(l => l.Language == language);
            if (localization.Parts != null && localization.Parts.Count > 0)
            {
                foreach (var part in localization.Parts)
                {
                    var formatReplica = GetFormatReplica(part);
                    if (formatReplica[^1] != ' ')
                    {
                        formatReplica += " ";
                    }
                    text += formatReplica;
                }

                replicas.Text = text;
                replicas.Speed = _currentNode.TypingSpeed;
            }

            return replicas;
        }

        private bool MoveNext(int conditionIndex)
        {
            if (_config.TryFindNextNode(_currentNode.ID, conditionIndex, out var nextNode))
            {
                _currentNode = nextNode;
                return true;
            }

            return false;
        }

        private string GetFormatReplica(ReplicaPartSerialized part)
        {
            StringBuilder formattedText = new StringBuilder();

            var text = part.MessageText;

            if (part.Color != new Color() && part.Color != Color.white && part.Color.a != 0)
            {
                var hexCode = ColorUtility.ToHtmlStringRGBA(part.Color);
                text = $"<color=#{hexCode}>{text}</color>";
            }

            foreach (var markup in part.Markups)
            {
                if (markup != TextMarkup.Default)
                {
                    text = markup switch
                    {
                        TextMarkup.Bold => $"<b>{text}</b>",
                        TextMarkup.Italic => $"<i>{text}</i>",
                        TextMarkup.Underline => $"<u>{text}</u>",
                        TextMarkup.Strikethrough => $"<s>{text}</s>",
                        _ => text
                    };
                }
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