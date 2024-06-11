using Code.Data.StaticData;
using Code.Infrastructure.Audio.AudioSystem;
using Code.Infrastructure.DI;
using Code.Infrastructure.Save;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.UI.Enums;
using Code.UI.Hud.SettingsMenu.Audio;
using Code.UI.Hud.SettingsMenu.Graphic;
using Code.UI.Hud.SettingsMenu.Language;
using UnityEngine;

namespace Code.UI.Hud.SettingsMenu
{
    public class SettingsMenuPresenter : BaseMenuPresenter<SettingsMenuModel,SettingsMenuView>, IProgressWriter
    {
        [Header("Sub presenters")] 
        [SerializeField] private AudioPresenter _audioPresenter;
        [SerializeField] private LanguagePresenter _languagePresenter;
        [SerializeField] private GraphicPresenter _graphicPresenter;
        
        [Header("Services")]
        private InputService _inputService;
        
        [Header("Limiters")]
        private InteractionLimiter _interactionLimiter;
        private MoveLimiter _moveLimiter;
        private TextLimiter _textLimiter;
        
        protected override void Init()
        {
            _inputService = Container.Instance.FindService<InputService>();
            
            _audioPresenter.Init(Container.Instance.FindService<AudioGlobalVolume>());
            _languagePresenter.Init(Container.Instance.FindService<LanguageSetter>());
            _graphicPresenter.Init(Container.Instance.FindService<ResolutionService>());
            
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _interactionLimiter = Container.Instance.FindService<InteractionLimiter>();
            _textLimiter = Container.Instance.FindService<TextLimiter>();
        }
        
        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _inputService.OnPressPauseKey += SwitchViewState;
                View.OnClickContinue += OnClickContinue;
                View.OnClickExit += OnClickExit;
            }
            else
            {
                _inputService.OnPressPauseKey -= SwitchViewState;
                View.OnClickContinue -= OnClickContinue;
                View.OnClickExit -= OnClickExit;
            }
            
            _audioPresenter.SubscribeToEvents(flag);
            _languagePresenter.SubscribeToEvents(flag);
            _graphicPresenter.SubscribeToEvents(flag);
        }

        private void OnClickExit()
        {
            Application.Quit();
        }

        private void OnClickContinue()
        {
            SwitchViewState();
        }

        private void SwitchViewState()
        {
            if (Model.IsValidating)
            {
                ChangeMenuState(MenuState.Inactive, onComplete: () =>
                {
                    _interactionLimiter.Unblock();
                    _moveLimiter.Unblock();    
                    _textLimiter.Unblock();
                });
            }
            else
            {
                _graphicPresenter.SetValues();
                ChangeMenuState(MenuState.Active );
                _interactionLimiter.Block();
                _moveLimiter.Block();    
                _textLimiter.Block();
            }
        }

        public void LoadProgress(SavedData playerProgress)
        {
            _audioPresenter.SetValues(playerProgress.AudioVolume);
            _languagePresenter.SetLanguage(playerProgress.Language);
          
        }

        public void UpdateProgress(SavedData playerProgress)
        {
            playerProgress.AudioVolume = _audioPresenter.AudioVolumeData;
            playerProgress.Language = _languagePresenter.Language;
        }
    }
}