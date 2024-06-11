using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSelector : MonoBehaviour
{
    [SerializeField] private GameObject _selectorButton;
    [SerializeField] private GameObject _selectorCanvasBoard;
    public List<Button> mybutton;
    private  ResolutionService _resolutionService;
    private  int id;

    void Start()
    {
       _resolutionService = GetComponent<ResolutionService>();
       
        CreateButton();
    }

    public void CreateButton()
    {
        id = 0;
        foreach (Vector2 r in Screen.fullScreen ? _resolutionService.FullscreenResolutions : _resolutionService.WindowedResolutions)
        {
            string label = r.x + "x" + r.y;
            if (r.x == Screen.width && r.y == Screen.height) label += "*";
            if (r.x == _resolutionService.GraphicData.DisplayResolution.width && r.y == _resolutionService.GraphicData.DisplayResolution.height) label += " (native)";

            GameObject newButton =  Instantiate(_selectorButton , _selectorCanvasBoard.transform.position, transform.rotation);
            newButton.GetComponentInChildren<TextMeshProUGUI>().SetText(label);
 
             newButton.transform.SetParent(_selectorCanvasBoard.transform, false);
            mybutton.Add(newButton.GetComponent<Button>());
            int i = id;
            mybutton[id].onClick.AddListener(() => {
                Debug.Log($"set resolution # {i}");
                _resolutionService.SetResolution(i, Screen.fullScreen);
            });
            id++;
        }
    }

    public void FullScreen(bool fullscreen)
    {
        _resolutionService.ToggleFullscreen();
    }



    private void OnEnable()
    {
        PlayerPrefs.DeleteAll();
    }
}
