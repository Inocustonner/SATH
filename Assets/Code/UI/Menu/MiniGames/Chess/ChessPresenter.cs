using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.Utils;
using UnityEngine;

namespace Code.UI.Menu.MiniGames.Chess
{
    public class ChessPresenter : BaseMenuPresenter<ChessModel, ChessView>
    {
        private ChessBoard _chessBoard;
        private InputService _inputService;
        private Vector2Int _selectedPosition;

        protected override void Init()
        {
            _chessBoard = new ChessBoard();
            View.InitializeBoard(8);

            _inputService = Container.Instance.FindService<InputService>();

            OnBoardChanged();
            _selectedPosition = new Vector2Int(0, 0);
            View.HighlightTile(_selectedPosition);
            base.Init();
        }

        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _inputService.OnPressMove += OnPressMove;
                _inputService.OnPressInteractionKey += OnPressInteractionKey;
                _chessBoard.OnBoardChanged += OnBoardChanged;
                _chessBoard.OnWin += OnWin;
                _chessBoard.OnLose += OnLose;
            }
            else
            {
                _inputService.OnPressMove -= OnPressMove;
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
                _chessBoard.OnBoardChanged -= OnBoardChanged;
                _chessBoard.OnWin -= OnWin;
                _chessBoard.OnLose -= OnLose;
            }
        }

        private void OnPressMove(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                Vector2Int moveDirection = new Vector2Int((int)direction.x, (int)direction.y);
                Vector2Int newPosition = _selectedPosition + moveDirection;

                this.Log($"press direction {direction} | new position {newPosition} | is valis {IsValidPosition(newPosition)}", Color.cyan);
                if (IsValidPosition(newPosition))
                {
                    _selectedPosition = newPosition;
                    View.HighlightTile(_selectedPosition);
                }
            }
        }

        private void OnPressInteractionKey()
        {
            if (!_chessBoard.SelectPiece(_selectedPosition))
            {
                _chessBoard.MoveSelectedPiece(_selectedPosition);
            }
        }

        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
        }

        private void OnBoardChanged()
        {
            View.UpdateBoard(_chessBoard.Board);
        }

        private void OnWin()
        {
            Debug.Log("You Win!");
            // Логика победы
        }

        private void OnLose()
        {
            Debug.Log("You Lose!");
            // Логика поражения
            _chessBoard = new ChessBoard(); // Перезапуск игры
            OnBoardChanged();
        }
    }
}
