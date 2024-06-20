using System;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.MiniGames.SlidingPuzzle
{
    public class SlidingPuzzleView: BaseMenuView
    {
        [SerializeField] private SlidingPuzzleTile _tilePrefab;
        [SerializeField] private Transform _boardParent;

        private int _size;
        private SlidingPuzzleTile[,] _tiles;
        [SerializeField]private SlidingPuzzleTile _selectedTile;
        
        public void SetMapSize(int size)
        {
            _size = size;
            _tiles = new SlidingPuzzleTile[_size, _size];
            CreateBoard();
        }
        
        public override void OpenMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        public void UpdateBoard(int[,] board)
        {
            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    int value = board[x, y];
                    var tile = _tiles[x, y];
                    tile.gameObject.SetActive(value != 0);
                    tile.SetText(value.ToString());
                }
            }
        }

        public void HighlightTile(Vector2Int position)
        {
            if (_selectedTile != null)
            {
                _selectedTile.SetColor(isSelected: false); 
            }

            _selectedTile = _tiles[position.x, position.y];
            _selectedTile.SetColor(isSelected: true); 
        }

        private void CreateBoard()
        {
            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    var tile = Instantiate(_tilePrefab, _boardParent);
                    tile.SetAnchoredPosition(new Vector2(x * 100, y * 100));
                    tile.SetColor(isSelected: false); 
                    tile.SetTilePosition(new Vector2Int(x,y));
                    _tiles[x, y] = tile;
                }
            }
        }
    }
}