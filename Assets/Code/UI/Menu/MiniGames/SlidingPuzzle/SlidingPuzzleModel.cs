using System;
using System.Collections.Generic;
using Code.UI.Base;
using UnityEngine;

namespace Code.UI.MiniGames.SlidingPuzzle
{
    public class SlidingPuzzleModel : BaseMenuModel<SlidingPuzzleModel>
    {
        public int[,] Board { get; private set; }
        public int Size { get; private set; }
        public Vector2Int EmptyTilePosition{ get; private set; }

        public event Action OnBoardChanged;
        public event Action OnWin;

        public void SetMapSize(int size)
        {
            Size = size;
            Board = new int[Size, Size];
            InitializeBoard();
            ShuffleBoard();
        }

        private void InitializeBoard()
        {
            int value = 1;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    Board[x, y] = value;
                    value++;
                }
            }
            Board[Size - 1, Size - 1] = 0; // Пустая ячейка
            EmptyTilePosition = new Vector2Int(Size - 1, Size - 1);
            OnBoardChanged?.Invoke();
        }

        private void ShuffleBoard()
        {
            int shuffleMoves = Size * Size * 10;
            System.Random rand = new System.Random();
            for (int i = 0; i < shuffleMoves; i++)
            {
                Vector2Int[] directions = new Vector2Int[]
                {
                    new Vector2Int(1, 0), // вправо
                    new Vector2Int(-1, 0), // влево
                    new Vector2Int(0, 1), // вверх
                    new Vector2Int(0, -1) // вниз
                };

                Vector2Int randomDirection = directions[rand.Next(directions.Length)];
                Vector2Int newPosition = EmptyTilePosition + randomDirection;

                if (IsValidPosition(newPosition))
                {
                    SwapTiles(EmptyTilePosition, newPosition);
                    EmptyTilePosition = newPosition;
                }
            }
            OnBoardChanged?.Invoke();
        }

        public bool Move(Vector2Int selectedTile)
        {
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 0), // вправо
                new Vector2Int(-1, 0), // влево
                new Vector2Int(0, 1), // вверх
                new Vector2Int(0, -1) // вниз
            };

            foreach (var direction in directions)
            {
                Vector2Int neighborPosition = selectedTile + direction;
                if (IsValidPosition(neighborPosition) && Board[neighborPosition.x, neighborPosition.y] == 0)
                {
                    SwapTiles(selectedTile, neighborPosition);
                    EmptyTilePosition = selectedTile; // Обновление позиции пустой клетки
                    OnBoardChanged?.Invoke();
                    if (CheckWinCondition())
                    {
                        OnWin?.Invoke();
                    }
                    return true;
                }
            }

            return false;
        }

        private bool CheckWinCondition()
        {
            int value = 1;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == Size - 1 && y == Size - 1)
                    {
                        return Board[x, y] == 0;
                    }
                    if (Board[x, y] != value)
                    {
                        return false;
                    }
                    value++;
                }
            }
            return false;
        }

        private void SwapTiles(Vector2Int pos1, Vector2Int pos2)
        {
            int temp = Board[pos1.x, pos1.y];
            Board[pos1.x, pos1.y] = Board[pos2.x, pos2.y];
            Board[pos2.x, pos2.y] = temp;
        }

        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < Size && position.y >= 0 && position.y < Size;
        }
    }
}
