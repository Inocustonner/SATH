using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MiniGames.SlidingPuzzle
{
    public class SlidingPuzzleTile : MonoBehaviour
    {
        [SerializeField] private RectTransform _body;
        [SerializeField] private Image _colorImage;
        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private Color _selectedColor, _defaultColor;

        public Vector2 GetSize()
        {
            return _body == null ? Vector2.zero : _body.sizeDelta;
        }
        
        public void SetText(string number)
        {
            _numberText.SetText(number);
        }

        public void SetColor(bool isSelected)
        {
            _colorImage.color = isSelected ? _selectedColor : _defaultColor;
        }


        public void SetAnchoredPosition(Vector2 pos)
        {
            _body.anchoredPosition = pos;
        }
    }
}