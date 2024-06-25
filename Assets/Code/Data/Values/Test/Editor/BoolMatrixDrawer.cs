using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BoolMatrix))]
public class BoolMatrixDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var matrixProperty = property.FindPropertyRelative("Matrix");
        int rows = matrixProperty.arraySize;

        if (rows > 0)
        {
            int cols = matrixProperty.GetArrayElementAtIndex(0).FindPropertyRelative("Array").arraySize;

            float cellSize = 20f;
            float spacing = 5f;

            for (int i = 0; i < rows; i++)
            {
                var rowProperty = matrixProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Array");

                for (int j = 0; j < cols; j++)
                {
                    var cellRect = new Rect(position.x + (cellSize + spacing) * j, position.y + (cellSize + spacing) * i, cellSize, cellSize);
                    var cellProperty = rowProperty.GetArrayElementAtIndex(j);
                    cellProperty.boolValue = EditorGUI.Toggle(cellRect, cellProperty.boolValue);
                }
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var matrixProperty = property.FindPropertyRelative("Matrix");
        int rows = matrixProperty.arraySize;
        float cellSize = 20f;
        float spacing = 5f;
        return rows * (cellSize + spacing);
    }
}