using System;
using System.Collections.Generic;
using Code.Data.Configs;
using Code.Data.Enums;
using UnityEngine;

namespace Code.Replicas.Scripts
{
    [Serializable]
    public struct ReplicaNodeSerialized
    {
        public string ID;
        public float TypingSpeed;
        public List<GameCondition> Conditions;
        public List<LocalizationSerialized> Localization;
        public Vector2 EditorPosition;
    }
    
    [Serializable]
    public struct LocalizationSerialized
    {
        public Lan Language;
        public List<ReplicaPartSerialized> Parts;
    }
    
    [Serializable]
    public struct ReplicaPartSerialized
    {
        public TextMarkup Markup;
        public TextEffect Effect;
        public Color Color; 
        public string MessageText;
    }
}