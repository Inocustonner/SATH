using System;
using System.Collections.Generic;
using System.Linq;
using Code.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud.SettingsMenu.Graphic
{
    [Serializable]
    public class GraphicPresenter
    {
        [SerializeField] private GraphicView _view;
        private ResolutionService _resolutionService;
        
        public void Init(ResolutionService resolutionService)
        {
            _resolutionService = resolutionService;
            _view.Init();
        }

        public void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _resolutionService.OnInitResolutions += OnInitResolutions;
                _view.OnSetResolution += OnPressSetResolution;
                _view.OnChangeFullScreen += ViewOnOnChangeFullScreen;
            }
            else
            {
                _resolutionService.OnInitResolutions -= OnInitResolutions;
                _view.OnSetResolution -= OnPressSetResolution;
                _view.OnChangeFullScreen -= ViewOnOnChangeFullScreen;
            }
        }

        private void OnInitResolutions()
        {
            SetResolutionValues();
        }

        private void ViewOnOnChangeFullScreen(bool isFull)
        {
            this.Log($"view event ChangeFullScreen");
            _resolutionService.SetFullscreen( isFull);
        }

        private void OnPressSetResolution(int index)
        {
            _resolutionService.SetResolution(index, _resolutionService.GraphicData.IsFullScreen);
        }
        
        public void SetResolutionValues()
        {
            List<string> labels = new List<string>();
            foreach (Vector2 resolution in _resolutionService.WindowedResolutions)
            {
                string label = resolution.x + "x" + resolution.y;
                if (resolution.x == Screen.width && resolution.y == Screen.height)
                {
                    label += "*";
                }

                if (resolution.x == _resolutionService.GraphicData.DisplayResolution.width &&
                    resolution.y == _resolutionService.GraphicData.DisplayResolution.height)
                {
                    label += " (native)";
                }

                labels.Add(label);
            }

            _view.SetDropdownValues(labels);
        }
    }

    [Serializable]
    public class GraphicView
    {
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private Toggle _toggle;
        
        public event Action<int> OnSetResolution;
        public event Action<bool> OnChangeFullScreen;

        public void Init()
        {
            _resolutionDropdown.onValueChanged.AddListener(SetResolution);
            _toggle.onValueChanged.AddListener(ChangeFullScreen);
        }

        private void ChangeFullScreen(bool isFull)
        {
            this.Log($"button event ChangeFullScreen");
            OnChangeFullScreen?.Invoke(isFull);
        }

        public void SetDropdownValues(IEnumerable<string> labels)
        {
            _resolutionDropdown.ClearOptions();
            var newOptions = labels.Select(label => new TMP_Dropdown.OptionData(label)).ToList();
            _resolutionDropdown.AddOptions(newOptions);
        }

        private void SetResolution(int index)
        {
            OnSetResolution?.Invoke(index);
        }
    }
}