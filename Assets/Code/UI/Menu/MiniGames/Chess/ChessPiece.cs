using UnityEngine;
namespace Code.UI.Menu.MiniGames.Chess
{

    public enum ChessPieceType { King, Queen, Rook, Bishop, Knight, Pawn }

    public class ChessPiece
    {
        public ChessPieceType Type { get; private set; }
        public bool IsWhite { get; private set; }
        public Vector2Int Position { get; set; }

        public ChessPiece(ChessPieceType type, bool isWhite, Vector2Int position)
        {
            Type = type;
            IsWhite = isWhite;
            Position = position;
        }

        public bool IsMoveValid(Vector2Int targetPosition, ChessPiece[,] board)
        {
            // Реализовать правила шахмат для каждой фигуры
            // Например:
            switch (Type)
            {
                case ChessPieceType.King:
                    return Mathf.Abs(Position.x - targetPosition.x) <= 1 && Mathf.Abs(Position.y - targetPosition.y) <= 1;
                case ChessPieceType.Queen:
                    return IsStraightMove(targetPosition, board) || IsDiagonalMove(targetPosition, board);
                case ChessPieceType.Rook:
                    return IsStraightMove(targetPosition, board);
                case ChessPieceType.Bishop:
                    return IsDiagonalMove(targetPosition, board);
                case ChessPieceType.Knight:
                    return Mathf.Abs(Position.x - targetPosition.x) * Mathf.Abs(Position.y - targetPosition.y) == 2;
                case ChessPieceType.Pawn:
                    // Добавить логику для пешек
                    break;
            }
            return false;
        }

        private bool IsStraightMove(Vector2Int targetPosition, ChessPiece[,] board)
        {
            if (Position.x != targetPosition.x && Position.y != targetPosition.y)
                return false;
            // Проверка на наличие препятствий на пути
            return true;
        }

        private bool IsDiagonalMove(Vector2Int targetPosition, ChessPiece[,] board)
        {
            if (Mathf.Abs(Position.x - targetPosition.x) != Mathf.Abs(Position.y - targetPosition.y))
                return false;
            // Проверка на наличие препятствий на пути
            return true;
        }
    }

}