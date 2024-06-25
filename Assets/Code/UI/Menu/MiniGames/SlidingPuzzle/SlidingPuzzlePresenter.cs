using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.Utils;
using UnityEngine;

namespace Code.UI.MiniGames.SlidingPuzzle
{
    public class SlidingPuzzlePresenter : BaseMenuPresenter<SlidingPuzzleModel, SlidingPuzzleView>
    {
        private const int MAP_SIZE = 4;
        
        private InputService _inputService;
        private Vector2Int _selectedPosition;

        protected override void Init()
        {
            _inputService = Container.Instance.FindService<InputService>();
          
            Model.SetMapSize(MAP_SIZE);
            View.SetMapSize(MAP_SIZE);

            OnBoardChanged();
            
            _selectedPosition = new Vector2Int(0, 0);
            View.HighlightTile(_selectedPosition);
        }
        
        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _inputService.OnPressInteractionKey += OnPressInteractionKey;
                _inputService.OnPressMove += OnPressMoveKey; 
                Model.OnBoardChanged += OnBoardChanged;
                Model.OnWin += OnWin;
            }
            else
            {
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
                _inputService.OnPressMove -= OnPressMoveKey; 
                Model.OnBoardChanged -= OnBoardChanged;
                Model.OnWin -= OnWin;
            }
        }

        private void OnWin()
        {
          this.Log("WIN",Color.yellow);
        }

        private void OnPressMoveKey(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                Vector2Int moveDirection = new Vector2Int((int)direction.x, (int)direction.y);
                Vector2Int newPosition = _selectedPosition + moveDirection;

                if (IsValidPosition(newPosition) && newPosition != Model.EmptyTilePosition)
                {
                    _selectedPosition = newPosition;
                    View.HighlightTile(_selectedPosition);
                    return;
                }

                newPosition += moveDirection;
                if (IsValidPosition(newPosition) && newPosition != Model.EmptyTilePosition)
                {
                    _selectedPosition = newPosition;
                    View.HighlightTile(_selectedPosition);
                }
            }
        }
        
        private bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < Model.Size && position.y >= 0 && position.y < Model.Size;
        }

        private void OnPressInteractionKey()
        {
            Model.Move(_selectedPosition);
        }
        
        private void OnBoardChanged()
        {
            View.UpdateBoard(Model.Board);
        }
    }
}