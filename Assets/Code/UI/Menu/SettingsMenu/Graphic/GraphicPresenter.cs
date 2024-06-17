using System;
using System.Collections.Generic;
using Code.Infrastructure.ScreenResolution;
using Code.Utils;
using UnityEngine;

namespace Code.UI.Menu.SettingsMenu.Graphic
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
            var labels = new List<string>();
            
            foreach (Vector2 resolution in _resolutionService.WindowedResolutions)
            {
                var label = resolution.x + "x" + resolution.y;
               
                /*if (resolution.x == Screen.width && resolution.y == Screen.height)
                {
                    label += "*";
                }*/

                /*if (resolution.x == _resolutionService.GraphicData.DisplayResolution.width &&
                    resolution.y == _resolutionService.GraphicData.DisplayResolution.height)
                {
                    label += " (native)";
                }*/
                
                labels.Add(label);
            }

            _view.SetResolutionValues(labels,_resolutionService.GraphicData.Index);
            _view.SetFullScreenValue(_resolutionService.GraphicData.IsFullScreen);
        }
    }
}