using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.UILine
{

    //https://www.youtube.com/watch?v=--LB7URk60A&ab_channel=GameDevGuide
    public class UILineRenderer : Graphic
    {
        [SerializeField] private float _thickness = 10;
        [SerializeField] private UIGridRenderer _grid;
        [SerializeField] private Vector2Int _gridSize;
        [Space]
        [SerializeField] private List<Vector2> _points;
        
        private float _width;
        private float _height;
        private float _unitWidth;
        private float _unitHeight;
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            _gridSize = _grid.Size;
            
            _width = rectTransform.rect.width;
            _height = rectTransform.rect.height;

            _unitWidth = _width / (float)_gridSize.x;
            _unitHeight = _height / (float)_gridSize.y;

            if (_points.Count < 2)
            {
                return;
            }

            float angle = 0;
            for (int i = 0; i < _points.Count; i++)
            {
                Vector2 point = _points[i];
                if (i < _points.Count - 1)
                {
                    angle = GetAngle(_points[i], _points[i + 1]);
                }
                DrawVerticesForPoint(point,angle, vh);
            }

            for (int i = 0; i < _points.Count - 1; i++)
            {
                int index = i * 2;
                vh.AddTriangle(index + 0, index + 1, index + 3);
                vh.AddTriangle(index + 3, index + 2, index + 0);
            }
        }

        public float GetAngle(Vector2 a, Vector2 b)
        {
            return (float)(Mathf.Atan2(b.y - a.y, b.x - a.x) * (180 / Mathf.PI));
        }
        private void DrawVerticesForPoint(Vector2 point, float angle, VertexHelper vh)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            vertex.position = Quaternion.Euler(0,0,angle) * new Vector3(-_thickness / 2, 0);
            vertex.position += new Vector3(_unitWidth * point.x, _unitHeight * point.y);
            vh.AddVert(vertex);
            
            vertex.position = Quaternion.Euler(0,0,angle) * new Vector3(_thickness / 2, 0);
            vertex.position += new Vector3(_unitWidth * point.x, _unitHeight * point.y);
            vh.AddVert(vertex);
        }

        private void Update()
        {
            if (_grid != null)
            {
                if (_gridSize != _grid.Size)
                {
                    SetVerticesDirty();
                }
            }
        }

        public List<Vector2> GetPoints()
        {
            return _points;
        }

        public void ResetPoints()
        {
            _points = new List<Vector2>();
        }
    }
}