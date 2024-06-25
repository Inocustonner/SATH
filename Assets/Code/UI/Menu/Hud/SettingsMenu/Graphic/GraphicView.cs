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

        public void SetResolutionValues(IEnumerable<string> labels, int current = 0)
        {
            _resolutionDropdown.ClearOptions();
            var newOptions = labels.Select(label => new TMP_Dropdown.OptionData(label)).ToList();
            _resolutionDropdown.AddOptions(newOptions);
            _resolutionDropdown.SetValueWithoutNotify(current);
        }

        public void SetFullScreenValue(bool isFullScreen)
        {
            _toggle.SetIsOnWithoutNotify(isFullScreen);
        }

        private void ChangeFullScreen(bool isFull)
        {
            this.Log($"button event ChangeFullScreen");
            OnChangeFullScreen?.Invoke(isFull);
        }

        private void SetResolution(int index)
        {
            OnSetResolution?.Invoke(index);
        }
    }
}