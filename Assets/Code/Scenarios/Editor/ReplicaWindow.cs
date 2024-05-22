using UnityEditor;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public class ReplicaWindow : EditorWindow
    {
        private ReplicaGraph graph;
        
        private void OnEnable()
        {
            AddDialogueGraph();
            AddToolbar();
        }

        private void AddDialogueGraph()
        {
            graph = new ReplicaGraph();
            graph.StretchToParentSize();
            rootVisualElement.Add(graph);
        }

        private void AddToolbar()
        {
            ReplicaToolbar toolbar = new ReplicaToolbar(graph);
            rootVisualElement.Add(toolbar);
        }

        [MenuItem("Lessons/Show Replica Window")]
        public static void ShowDialogueWindow()
        {
            GetWindow<ReplicaWindow>("Replica Window");
        }
    }
}