using Code.Infrastructure.Audio.AudioSystem;
using Code.Infrastructure.DI;
using Code.Infrastructure.Save;
using Code.Infrastructure.Services;
using Code.UI.Base;
using Code.UI.Hud.SettingsMenu.Audio;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.UI.Hud.SettingsMenu
{
    public class SettingsMenuPresenter : BaseMenuPresenter<SettingsMenuModel,SettingsMenuView>, IProgressWriter
    {
        [Header("Components")] 
        [SerializeField] private AudioPresenter _audioPresenter;
        
        [Header("Services")]
        private InputService _inputService;
        private InteractionLimiter _interactionLimiter;
        private MoveLimiter _moveLimiter;
        
        protected override void Init()
        {
            _audioPresenter.Init(Container.Instance.FindService<SceneAudioController>()); 
            _inputService = Container.Instance.FindService<InputService>();
            _moveLimiter = Container.Instance.FindService<MoveLimiter>();
            _interactionLimiter = Container.Instance.FindService<InteractionLimiter>();
        }
        
        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _inputService.OnPressPauseKey += OnPressPauseKey;
            }
            else
            {
                _inputService.OnPressPauseKey -= OnPressPauseKey;
            }
            
            _audioPresenter.SubscribeToEvents(flag);
        }

        private void OnPressPauseKey()
        {
            if (Model.IsValidating)
            {
                ChangeMenuState(MenuState.Inactive, onComplete: () =>
                {
                    _interactionLimiter.Unblock();
                    _moveLimiter.Unblock();    
                });
            }
            else
            {
                ChangeMenuState(MenuState.Active );
                _interactionLimiter.Block();
                _moveLimiter.Block();    
            }
        }

        public void LoadProgress(SavedData playerProgress)
        {
            _audioPresenter.SetValues(playerProgress.AudioVolume);
        }

        public void UpdateProgress(SavedData playerProgress)
        {
            playerProgress.AudioVolume = _audioPresenter.AudioVolumeData;
        }
    }
}