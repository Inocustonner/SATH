using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace Code.UI.Menu.MiniGames.Chess
{
    public class ChessPresenter : BaseMenuPresenter<ChessModel, ChessView>
    {
        private InputService _inputService;
        private Vector2Int _selectedPosition;
        private List<Vector2Int> _validMoves;

        protected override void Init()
        {
            Model.InitializeBoard();
            View.InitializeBoard(8);

            _inputService = Container.Instance.FindService<InputService>();

            OnModelChanged();
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
                Model.OnBoardChanged += OnModelChanged;
                Model.OnWin += OnWin;
                Model.OnLose += OnLose;
            }
            else
            {
                _inputService.OnPressMove -= OnPressMove;
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
                Model.OnBoardChanged -= OnModelChanged;
                Model.OnWin -= OnWin;
                Model.OnLose -= OnLose;
            }
        }

        private void OnPressMove(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                Vector2Int moveDirection = new Vector2Int((int)direction.x, (int)direction.y);
                Vector2Int newPosition = _selectedPosition + moveDirection;

                if (IsValidPosition(newPosition))
                {
                    _selectedPosition = newPosition;
                    View.HighlightTile(_selectedPosition);
                    // Подсветим возможные ходы, если фигура выбрана
                    if (_validMoves != null && _validMoves.Count > 0)
                    {
                        View.HighlightMoves(_validMoves);
                    }
                }
            }
        }

        private void OnPressInteractionKey()
        {
            if (Model.SelectPiece(_selectedPosition))
            {
                _validMoves = Model.GetValidMoves(_selectedPosition);
                View.HighlightMoves(_validMoves);
            }
            else if (_validMoves != null && _validMoves.Contains(_selectedPosition))
            {
                Model.MoveSelectedPiece(_selectedPosition);
                View.ClearHighlights();
                _validMoves = null; // Сбросим список возможных ходов после выполнения хода
            }
            else
            {
                View.ClearHighlights();
                _validMoves = null; // Сбросим список возможных ходов при снятии выделения
            }
        }

        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
        }

        private void OnModelChanged()
        {
            View.UpdateBoard(Model.Board);
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
            /*Model.InitializeBoard(); // Перезапуск игры
            OnModelChanged();*/
        }
    }
}
