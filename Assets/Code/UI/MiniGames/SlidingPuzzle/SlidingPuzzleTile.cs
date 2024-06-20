using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MiniGames.SlidingPuzzle
{
    public class SlidingPuzzleTile : MonoBehaviour
    {
        [SerializeField] private RectTransform _body;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Color _selectedColor, _defaultColor;

        public Vector2Int TilePosition { get; private set; }

        public void SetText(string number)
        {
            _numberText.SetText(number);
        }

        public void SetColor(bool isSelected)
        {
            _image.color = isSelected ? _selectedColor : _defaultColor;
        }

        public void SetTilePosition(Vector2Int tilePosition)
        {
            TilePosition = tilePosition;
        }
        public void SetAnchoredPosition(Vector2 pos)
        {
            _body.anchoredPosition = pos;
        }
    }
}