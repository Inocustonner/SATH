using System;
using System.Collections.Generic;
using Code.Data.Enums;
using UnityEngine;

namespace Code.Scenarios.Scripts
{
    [Serializable]
    public struct DialogueNodeSerialized
    {
        public string ID;
        public string Message;
        public List<TextCondition> Conditions;
        public Vector2 EditorPosition;
    }
}