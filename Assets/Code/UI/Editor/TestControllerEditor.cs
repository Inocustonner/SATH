using UnityEditor;
using UnityEngine;

namespace Code.UI.Editor
{
    [CustomEditor(typeof(AnimatedText))]
    public class TestControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            AnimatedText animatedText = (AnimatedText)target;

            //if (GUILayout.Button("Show Text")) characterAnimator.StartWrite();

        }
    }
}