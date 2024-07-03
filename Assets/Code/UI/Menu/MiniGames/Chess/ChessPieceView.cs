using Code.UI.Menu.MiniGames.Chess;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessPieceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [SerializeField] private Image _background;

    private Color _defaultColor = Color.clear;
    private Color _selectedColor = Color.red;
    private Color _highlightColor = Color.yellow;

    public void SetPiece(ChessPiece piece)
    {
        if (piece == null)
        {
            _image.enabled = false;
            _text.text = "";
        }
        else
        {
            _image.enabled = true;
            _text.SetText(piece.Type.ToString());
            _image.color = piece.IsWhite ? Color.white : Color.black;
        }

        gameObject.name = "ChessPiece " + _text.text;
        SetDefault();
    }

    public void SetDefault()
    {
        _background.color = _defaultColor;
    }

    public void SetSelected()
    {
        _background.color = _selectedColor;
    }

    public void SetHighlighted()
    {
        _background.color = _highlightColor;
    }
}