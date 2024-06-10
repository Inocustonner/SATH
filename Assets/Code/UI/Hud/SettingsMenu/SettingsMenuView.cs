using System;
using Code.UI.Base;
using Code.UI.Components;
using UnityEngine;

namespace Code.UI.Hud.SettingsMenu
{
    public class SettingsMenuView : BaseMenuView
    {
        [Header("Components")] 
        [SerializeField] private EventButton _continueButton;
        [SerializeField] private EventButton _exitButton;
        [SerializeField] private EventButton _settingsButton;
        [SerializeField] private GameObject _settingsView;

        private bool _isShowSettings;
        public event Action OnClickContinue;
        public event Action OnClickExit;
        
        public override void OpenMenu(Action onComplete = null)
        {
            SubscribeToEvents(true);
            windowTransform.gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public override void CloseMenu(Action onComplete = null)
        {
            SubscribeToEvents(false);
            windowTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _settingsButton.OnClickButton += ClickSettingsButton;
                _continueButton.OnClickButton += ClickContinueButton;
                _exitButton.OnClickButton += ClickExitButton;
            }
            else
            {
                _settingsButton.OnClickButton -= ClickSettingsButton;
                _continueButton.OnClickButton -= ClickContinueButton;
                _exitButton.OnClickButton -= ClickExitButton;
            }
        }

        private void ClickExitButton()
        {
            OnClickExit?.Invoke();
        }

        private void ClickContinueButton()
        {
            OnClickContinue?.Invoke();
        }

        private void ClickSettingsButton()
        {
            _isShowSettings = !_isShowSettings;
            _settingsView.SetActive(_isShowSettings);
        }
    }
}