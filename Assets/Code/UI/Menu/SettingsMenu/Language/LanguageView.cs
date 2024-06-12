using System;
using Code.Data.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menu.SettingsMenu.Language
{
    [Serializable]
    public class LanguageView
    {
        [SerializeField] private Button _rus;
        [SerializeField] private Button _eng;

        public event Action<Lan> OnPressButton; 
        public void Init(Lan language)
        {
            _rus.onClick.AddListener(() => SetLanguage(Lan.Rus));
            _eng.onClick.AddListener(() => SetLanguage(Lan.Eng));
            SetLanguage(language);
        }

        private void SetLanguage(Lan language)
        {
            if (language == Lan.Rus)
            {
                _rus.interactable = false;
                _eng.interactable = true;
            }
            else
            {
                _rus.interactable = true;
                _eng.interactable = false;
            }
            OnPressButton?.Invoke(language);
        }
    }
}