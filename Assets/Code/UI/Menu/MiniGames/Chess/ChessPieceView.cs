using Code.UI.Menu.MiniGames.Chess;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessPieceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [SerializeField] private Image _background;

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

        gameObject.name = "ChestPiece " + _text.text;
    }

    public void SetSelected(bool isSelected)
    {
        _background.color = isSelected ? Color.clear : Color.red;
    }
    

} 