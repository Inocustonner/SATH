using Code.UI.Hud.ReplicaMenu;
using UnityEditor;
using UnityEngine;

namespace Code.UI.Editor
{
    [CustomEditor(typeof(ReplicaTester))]
    public class ReplicaTesterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ReplicaTester text = (ReplicaTester)target;

            if (GUILayout.Button("Start Write")) text.StartWrite();
            if (GUILayout.Button("Skip")) text.Skip();

        }
    }
}