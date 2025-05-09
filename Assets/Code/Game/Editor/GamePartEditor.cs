﻿using Code.Game.Parts;
using UnityEditor;
using UnityEngine;

namespace Code.Game.Editor
{
    [CustomEditor(typeof(GamePart),true),CanEditMultipleObjects]
    public class GamePartEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GamePart gamePart = (GamePart)target;
            if (GUILayout.Button("Restart")) gamePart.Restart();
        }
    }
}