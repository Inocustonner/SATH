using System;
using System.Collections.Generic;
using Code.UI.Base;
using Code.UI.Menu.MiniGames.Chess;
using UnityEngine;
using UnityEngine.UI;

public class ChessView : BaseMenuView
{
    [SerializeField] private ChessPieceView _piecePrefab;
    [SerializeField] private Transform _boardParent;

    private ChessPieceView[,] _pieceViews;

    public void InitializeBoard(int size)
    {
        _pieceViews = new ChessPieceView[size, size];
        Vector2 sizePiece = new Vector2(112.5f, 112.5f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                var pieceView = Instantiate(_piecePrefab, _boardParent);
                pieceView.transform.localPosition = new Vector3(x * sizePiece.x, y * sizePiece.y, 0);
                _pieceViews[x, y] = pieceView;
            }
        }
    }

    public void UpdateBoard(ChessPiece[,] board)
    {
        for (int y = 0; y < board.GetLength(1); y++)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                _pieceViews[x, y].SetPiece(board[x, y]);
            }
        }
    }

    public void HighlightTile(Vector2Int position)
    {
        ClearHighlights();
        _pieceViews[position.x, position.y].SetSelected();
    }

    public void HighlightMoves(List<Vector2Int> moves)
    {
        foreach (var move in moves)
        {
            _pieceViews[move.x, move.y].SetHighlighted();
        }
    }

    public void ClearHighlights()
    {
        for (int y = 0; y < _pieceViews.GetLength(1); y++)
        {
            for (int x = 0; x < _pieceViews.GetLength(0); x++)
            {
                _pieceViews[x, y].SetDefault();
            }
        }
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
}
