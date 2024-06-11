using System;
using Code.Data.Enums;
using Code.Infrastructure.Services;
using Code.Utils;
using UnityEngine;

namespace Code.UI.Hud.SettingsMenu.Language
{
    [Serializable]
    public class LanguagePresenter
    {
        [SerializeField] private LanguageView _view;
        private LanguageSetter _languageSetter;
        public Lan Language { get; private set; }

        public void Init(LanguageSetter languageSetter)
        {
            _languageSetter = languageSetter;
        }

        public void SetLanguage(Lan lan)
        {
            Language = lan;
            _view.Init(lan);
            this.Log($"load {lan}");
        }

        public void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _view.OnPressButton += OnPressButton;
            }
            else
            {
                _view.OnPressButton -= OnPressButton;
            }
        }

        private void OnPressButton(Lan lan)
        {
            Language = lan;
            _languageSetter.SetLanguage(lan);
        }
    }
}