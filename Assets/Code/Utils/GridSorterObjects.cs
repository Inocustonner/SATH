using UnityEngine;

namespace Code.Utils
{
    public class GridSorterObjects : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Transform[] _gameObjects;

        [ContextMenu("Sort")]
        private void Sort()
        {
            for (int i = 0; i < _gameObjects.Length; i++)
            {
                _gameObjects[i].position = new Vector3(_grid.cellSize.x * i,0,0);
            }
        }
    }
}