using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.UILine
{
    public class UIGridRenderer : Graphic
    {
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(1, 1);
        [SerializeField] private float _thickness = 10;

        private float _width;
        private float _height;
        private float _cellWidth;
        private float _cellHeight;

        public float Width => _width;
       public float CellWidth => _cellWidth;
        public float CellHeight => _cellHeight;
        public Vector2Int Size => _gridSize;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            _width = rectTransform.rect.width;
            _height = rectTransform.rect.height;

            _cellWidth = _width / (float)_gridSize.x;
            _cellHeight = _height / (float)_gridSize.y;

            int count = 0;
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    DrawCell(x, y, count, vh);
                    count++;
                }
            }
        }

        private void DrawCell(int x, int y, int index, VertexHelper vh)
        {
            float xPos = _cellWidth * x;
            float yPos = _cellHeight * y;

            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            vertex.position = new Vector3(xPos, yPos);
            vh.AddVert(vertex);

            vertex.position = new Vector3(xPos, yPos + _cellHeight);
            vh.AddVert(vertex);

            vertex.position = new Vector3(xPos + _cellWidth, yPos + _cellHeight);
            vh.AddVert(vertex);

            vertex.position = new Vector3(xPos + _cellWidth, yPos);
            vh.AddVert(vertex);

            float widthSqr = _thickness * _thickness;
            float distanceSqr = widthSqr / 2;
            float distance = Mathf.Sqrt(distanceSqr);

            vertex.position = new Vector3(xPos + distance, yPos + distance);
            vh.AddVert(vertex);

            vertex.position = new Vector3(xPos + distance, yPos + (_cellHeight - distance));
            vh.AddVert(vertex);

            vertex.position = new Vector3(xPos + (_cellWidth - distance), yPos + (_cellHeight - distance));
            vh.AddVert(vertex);

            vertex.position = new Vector3(xPos + (_cellWidth - distance),yPos + distance);
            vh.AddVert(vertex);

            int offset = index * 8;
            //left edge
            vh.AddTriangle(offset + 0, offset + 1, offset + 5);
            vh.AddTriangle(offset + 5, offset + 4, offset + 0);

            //top edge
            vh.AddTriangle(offset + 1, offset +  2, offset +  6);
            vh.AddTriangle(offset + 6, offset +  5, offset +  1);

            //right edge
            vh.AddTriangle(offset + 2, offset + 3, offset + 7);
            vh.AddTriangle(offset + 7, offset + 6, offset + 2);

            //bottom edge
            vh.AddTriangle(offset + 3, offset + 0, offset + 4);
            vh.AddTriangle(offset + 4, offset + 7, offset + 3);
        }
    }
}