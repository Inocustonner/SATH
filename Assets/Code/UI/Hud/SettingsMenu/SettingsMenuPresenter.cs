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
        
        protected override void Init()
        {
            _audioPresenter.Init(Container.Instance.FindService<SceneAudioController>());
            _inputService = Container.Instance.FindService<InputService>();
            this.Log("Init");
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
            ChangeMenuState(Model.IsValidating ? MenuState.Inactive : MenuState.Active);
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