using UnityEditor;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class DialogueWindow : EditorWindow
    {
        private DialogueGraph graph;
        
        private void OnEnable()
        {
            AddDialogueGraph();
            AddToolbar();
        }

        private void AddDialogueGraph()
        {
            graph = new DialogueGraph();
            graph.StretchToParentSize();
            rootVisualElement.Add(graph);
        }

        private void AddToolbar()
        {
            DialogueToolbar toolbar = new DialogueToolbar(graph);
            rootVisualElement.Add(toolbar);
        }

        [MenuItem("Lessons/Show Dialogue Window")]
        public static void ShowDialogueWindow()
        {
            GetWindow<DialogueWindow>("Dialogue Window");
        }
    }
}