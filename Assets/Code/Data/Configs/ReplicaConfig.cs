using System;
using System.Linq;
using System.Text;
using Code.Data.Enums;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "Replica_", menuName = "Config/Replica", order = 0)]
    public class ReplicaConfig : ScriptableObject
    {
        public bool IsBlockMovement = true;
        public float WriteSpeed ;
        public LocalizedReplica[] Replicas;

        public bool TryGetLocalizedReplica(Lan language, out string replica)
        {
            replica = "";
            var replicaParts = Replicas.FirstOrDefault(r => r.Lan == language).ReplicaParts;
            if (replicaParts == null || replicaParts.Length == 0)
            {
                return false;
            }

            replica = GetFormatReplica(replicaParts);
            return replica != "";
        }

        private string GetFormatReplica(ReplicaPart[] parts)
        {
            StringBuilder formattedText = new StringBuilder();

            foreach (var part in parts)
            {
                string text = part.Text;

                if (part.Color != new Color() && part.Color != Color.white)
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
                        _ => text
                    };
                }

                if (part.Effect != TextEffect.Default)
                {
                    text = $"<{part.Effect.ToString().ToLower()}>{text}</{part.Effect.ToString().ToLower()}>";
                }

                formattedText.Append(text);
            }

            return formattedText.ToString();
        }

    }

    [Serializable]
    public struct LocalizedReplica
    {
        public Lan Lan;
        public ReplicaPart[] ReplicaParts;
    }
    
    [Serializable]
    public struct ReplicaPart
    {
        public string Text;
        public Color Color;
        public TextMarkup Markup;
        public TextEffect Effect;
    }
}