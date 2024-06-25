using System.Linq;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.Save;
using Code.Infrastructure.Services;
using Code.UI.Enums;
using TMPro;
using UnityEngine;

namespace Code.UI.Components
{
    public class LocalizationElement : MonoBehaviour, IGameInitListener,IGameStartListener,IGameExitListener, IProgressReader
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private LocalizationElementType _elementType;
        private LanguageSetter _languageSetter;
        private LocalizationElementData _localizationData;

        public void GameInit()
        {
            _languageSetter = Container.Instance.FindService<LanguageSetter>();
            _localizationData = Container.Instance.FindConfig<LocalizationElementsConfig>().LocalizationElements
                .FirstOrDefault(l => l.ElementType == _elementType);
        }

        public void GameStart()
        {
            SubscribeToEvents(true);   
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        public void LoadProgress(SavedData playerProgress)
        {
            SetLanguage();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _languageSetter.OnSwitchLanguage += SetLanguage;
            }
            else
            {
                _languageSetter.OnSwitchLanguage -= SetLanguage;
            }
        }

        private void SetLanguage()
        {
            var text = _localizationData.Localization.FirstOrDefault(l => l.Language == _languageSetter.Language).Text;
            _textMeshProUGUI.SetText(text);
        }
    }
}