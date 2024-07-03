using System;
using Code.UI.Menu.MiniGames.Chess;
using UnityEngine;

public class ChessBoard
{
    public ChessPiece[,] Board { get; private set; }
    public Vector2Int SelectedPiecePosition { get; private set; }
    private bool _isPieceSelected;

    public event Action OnBoardChanged;
    public event Action OnWin;
    public event Action OnLose;

    public ChessBoard()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        Board = new ChessPiece[8, 8];

        // Инициализация белых фигур
        Board[0, 5] = new ChessPiece(ChessPieceType.Rook, true, new Vector2Int(0, 5));
        Board[2, 7] = new ChessPiece(ChessPieceType.Rook, true, new Vector2Int(2, 7));
        Board[6, 6] = new ChessPiece(ChessPieceType.Knight, true, new Vector2Int(6, 6));
        Board[6, 2] = new ChessPiece(ChessPieceType.King, true, new Vector2Int(6, 2));
        Board[1, 1] = new ChessPiece(ChessPieceType.Queen, true, new Vector2Int(1, 1));
        Board[6, 3] = new ChessPiece(ChessPieceType.Queen, true, new Vector2Int(6, 3));

        // Инициализация черных фигур
        Board[3, 7] = new ChessPiece(ChessPieceType.Bishop, false, new Vector2Int(3, 7));
        Board[5, 7] = new ChessPiece(ChessPieceType.Rook, false, new Vector2Int(5, 7));
        Board[5, 6] = new ChessPiece(ChessPieceType.Knight, false, new Vector2Int(5, 6));
        Board[3, 4] = new ChessPiece(ChessPieceType.King, false, new Vector2Int(3, 4));
        Board[4, 4] = new ChessPiece(ChessPieceType.Pawn, false, new Vector2Int(4, 4));
        Board[4, 3] = new ChessPiece(ChessPieceType.Pawn, false, new Vector2Int(4, 3));
        Board[1, 3] = new ChessPiece(ChessPieceType.Pawn, false, new Vector2Int(1, 3));
        Board[3, 3] = new ChessPiece(ChessPieceType.Bishop, false, new Vector2Int(3, 3));
        Board[3, 2] = new ChessPiece(ChessPieceType.Knight, false, new Vector2Int(3, 2));

        OnBoardChanged?.Invoke();
    }

    public bool SelectPiece(Vector2Int position)
    {
        if (!_isPieceSelected && Board[position.x, position.y] != null && Board[position.x, position.y].IsWhite)
        {
            SelectedPiecePosition = position;
            _isPieceSelected = true;
            return true;
        }
        _isPieceSelected = false;
        return false;
    }

    public bool MoveSelectedPiece(Vector2Int targetPosition)
    {
        if (_isPieceSelected && Board[SelectedPiecePosition.x, SelectedPiecePosition.y].IsMoveValid(targetPosition, Board))
        {
            ChessPiece piece = Board[SelectedPiecePosition.x, SelectedPiecePosition.y];
            Board[SelectedPiecePosition.x, SelectedPiecePosition.y] = null;
            Board[targetPosition.x, targetPosition.y] = piece;
            piece.Position = targetPosition;
            _isPieceSelected = false;

            OnBoardChanged?.Invoke();

            if (piece.Type == ChessPieceType.Queen && targetPosition == new Vector2Int(1, 2))
            {
                OnWin?.Invoke();
            }
            else
            {
                OnLose?.Invoke();
            }

            return true;
        }
        return false;
    }
}
