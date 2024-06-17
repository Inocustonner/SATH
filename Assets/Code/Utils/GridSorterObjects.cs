using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Code.Utils
{
    public class GridSorterObjects : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private Transform _prefab;
        [SerializeField] private List<Transform> _gameObjects;

#if UNITY_EDITOR
        
        [ContextMenu("Sort")]
        private void Sort()
        {
            foreach (var gameObject in _gameObjects)
            {
                DestroyImmediate(gameObject);
            }
            _gameObjects.Clear();
            for (int i = 0; i < Mathf.CeilToInt(_backgroundRenderer.size.x/ _grid.cellSize.x); i++)
            {
                if (i >= _gameObjects.Count)
                {
                    var obj = (GameObject)PrefabUtility.InstantiatePrefab(_prefab.gameObject, transform);
                    _gameObjects.Add(obj.transform);
                }
                var x = -(_backgroundRenderer.size.x / 2) + _grid.cellSize.x  * i + _backgroundRenderer.transform.position.x;
                _gameObjects[i].position = new Vector3(x,0,0);
            }
        }

        private void OnDrawGizmos()
        {
            if (_backgroundRenderer == null)
            {
                return;
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < Mathf.CeilToInt(_backgroundRenderer.size.x/ _grid.cellSize.x); i++)
            {
                var x = -(_backgroundRenderer.size.x / 2) + _grid.cellSize.x  * i + _backgroundRenderer.transform.position.x;
                Gizmos.DrawLine(new Vector3(x,-15,0), new Vector3(x,15,0));
            }
        }

        
#endif
        
    }
}