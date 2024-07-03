using System;
using System.Collections.Generic;
using Code.UI.Base;
using Code.UI.Menu.MiniGames.Chess;
using UnityEngine;

public class ChessModel : BaseMenuModel<ChessModel>
{
    public ChessPiece[,] Board { get; private set; }
    [SerializeField] private Vector2Int _selectedPiecePosition;
    [SerializeField] private bool _isPieceSelected;
    public Vector2Int SelectedPiecePosition => _selectedPiecePosition;
    public event Action OnBoardChanged;
    public event Action OnWin;
    public event Action OnLose;

    public void InitializeBoard()
    {
        Board = new ChessPiece[8, 8];

        // Инициализация белых фигур
        Board[0, 5] = new ChessPiece(ChessPieceType.Rook, true, new Vector2Int(0, 5));
        Board[2, 7] = new ChessPiece(ChessPieceType.Rook, true, new Vector2Int(2, 7));
        Board[6, 1] = new ChessPiece(ChessPieceType.Knight, true, new Vector2Int(6, 1));
        Board[6, 2] = new ChessPiece(ChessPieceType.King, true, new Vector2Int(6, 2));
        Board[1, 0] = new ChessPiece(ChessPieceType.Queen, true, new Vector2Int(1, 0));
        Board[6, 5] = new ChessPiece(ChessPieceType.Queen, true, new Vector2Int(6, 5));

        // Инициализация черных фигур
        Board[3, 6] = new ChessPiece(ChessPieceType.Bishop, false, new Vector2Int(3, 6));
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
            _selectedPiecePosition = position;
            _isPieceSelected = true;
            Debug.Log($"Selected piece at {position}");
            return true;
        }

        if (_isPieceSelected && _selectedPiecePosition == position)
        {
            _isPieceSelected = false;
            Debug.Log($"Deselected piece at {position}");
            return false;
        }

        Debug.Log($"Invalid selection at {position}");
        return false;
    }

    public bool MoveSelectedPiece(Vector2Int targetPosition)
    {
        if (_isPieceSelected && Board[_selectedPiecePosition.x, _selectedPiecePosition.y].IsMoveValid(targetPosition, Board))
        {
            ChessPiece piece = Board[_selectedPiecePosition.x, _selectedPiecePosition.y];
            Board[_selectedPiecePosition.x, _selectedPiecePosition.y] = null;
            Board[targetPosition.x, targetPosition.y] = piece;
            piece.Position = targetPosition;
            _isPieceSelected = false;

            OnBoardChanged?.Invoke();
            Debug.Log($"Moved piece to {targetPosition}");

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

        Debug.Log($"Invalid move to {targetPosition}");
        return false;
    }

    public List<Vector2Int> GetValidMoves(Vector2Int piecePosition)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        ChessPiece piece = Board[piecePosition.x, piecePosition.y];

        if (piece != null)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Vector2Int targetPosition = new Vector2Int(x, y);
                    if (piece.IsMoveValid(targetPosition, Board))
                    {
                        validMoves.Add(targetPosition);
                    }
                }
            }
        }

        return validMoves;
    }
}
