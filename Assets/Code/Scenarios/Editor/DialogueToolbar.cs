#if UNITY_EDITOR
using Code.Scenarios.Scripts;
using Lessons.MetaGame.Dialogs;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Code.Scenarios.Editor
{
    public sealed class DialogueToolbar : Toolbar
    {
        public DialogueToolbar(DialogueGraph graph)
        {
            this.graph = graph;
            AddDialogField();
            AddLoadButton();
            AddSaveButton();
            AddResetButton();
        }

        private readonly DialogueGraph graph;
        private ObjectField dialogField;

        private void AddDialogField()
        {
            dialogField = new ObjectField("Selected Dialog")
            {
                objectType = typeof(DialogueConfig),
                allowSceneObjects = false
            };

            Add(dialogField);
        }

        private void AddLoadButton()
        {
            var button = new Button
            {
                text = "Load",
                clickable = new Clickable(OnLoadDialog)
            };

            Add(button);
        }

        private void AddSaveButton()
        {
            var button = new Button
            {
                text = "Save",
                clickable = new Clickable(OnSaveDialog)
            };

            Add(button);
        }

        private void AddResetButton()
        {
            var button = new Button
            {
                text = "Reset",
                clickable = new Clickable(OnResetDialog)
            };

            Add(button);
        }

        private void OnLoadDialog()
        {
            graph.Reset();

            var config = dialogField.value as DialogueConfig;
            /*
            if (config != null)
            {
                DialogueSaveLoader.SaveDialog(graph, config);
            }
            */

            DialogueSaveLoader.LoadDialog(graph, config);
        }

        private void OnSaveDialog()
        {
            var config = dialogField.value as DialogueConfig;
            if (config != null)
            {
                DialogueSaveLoader.SaveDialog(graph, config);
            }
            else
            {
                DialogueSaveLoader.CreateDialog(graph, out config);
                dialogField.value = config;
            }
        }

        private void OnResetDialog()
        {
            graph.Reset();
        }
    }
}
#endif