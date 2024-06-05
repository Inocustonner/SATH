using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Code.Utils
{
    public class GridSorterObjects : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _prefab;
        [SerializeField] private List<Transform> _gameObjects;


        [ContextMenu("Test")]
        private void Test()
        {
            this.Log($"{_spriteRenderer.size.x}    {_spriteRenderer.size.x/_grid.cellSize.x}"); 
        }
        
        [ContextMenu("Sort")]
        private void Sort()
        {
            
            for (int i = 0; i < Mathf.CeilToInt(_spriteRenderer.size.x/_grid.cellSize.x); i++)
            {
                if (i >= _gameObjects.Count)
                {
                    var obj = (GameObject)PrefabUtility.InstantiatePrefab(_prefab.gameObject, transform);
                    _gameObjects.Add(obj.transform);
                }
                _gameObjects[i].position = new Vector3(_grid.cellSize.x * i,0,0);
            }
        }
    }
}