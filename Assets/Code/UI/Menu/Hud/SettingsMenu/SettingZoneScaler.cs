using Code.Data.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Hud.SettingsMenu
{
    public class SettingZoneScaler : MonoBehaviour, IGameTickListener
    {
        [SerializeField] private VerticalLayoutGroup _settingsVerticalLayout;
        [SerializeField] private VerticalLayoutGroup _mainVerticalLayout;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private RectTransform _resolutionParent;
        [SerializeField] private RectTransform _settingsViewContent;
        [SerializeField] private float expandedHeight = 200f; 
        [SerializeField] private float collapsedHeight = 30f;
        private GameObject _dropdownList;
        
        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            SetParentHeight(_resolutionDropdown.IsExpanded ? expandedHeight : collapsedHeight);
        }

        private void SetParentHeight(float height)
        {
            Vector2 sizeDelta = _resolutionParent.sizeDelta;
            sizeDelta.y = height;
            _resolutionParent.sizeDelta = sizeDelta;
            _settingsVerticalLayout.SetLayoutVertical();
            _mainVerticalLayout.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_settingsViewContent);
        }
    }
}