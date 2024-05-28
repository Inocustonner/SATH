using System.Collections.Generic;
using Code.Infrastructure.Audio.AudioSystem;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Utils;
using Core.Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Code.Infrastructure.Save
{
    public class SaveLoadService: IService, IGameLoadListener, IGameExitListener
    {
        private const string progressKey = "Progress";

        private  SavedData _playerProgress;

        private List<IProgressWriter> _progressWriters = new();
        private List<IProgressReader> _progressReader = new();
        public void GameLoad()
        {
            _progressReader = Container.Instance.GetContainerComponents<IProgressReader>();
            foreach (var progressReader in _progressReader)
            {
                if (progressReader is IProgressWriter writer)
                {
                    _progressWriters.Add(writer);
                }
            }
            
            LoadProgress();
            
            this.Log($"Game load");
        }

        public void GameExit()
        {
            SaveProgress();
        }

        private void SaveProgress()
        {
            foreach (var progressWriter in  _progressWriters)
            {
                progressWriter.UpdateProgress(_playerProgress);
            }

            PlayerPrefs.SetString(progressKey, _playerProgress.ToJson());
                 
            var data = PlayerPrefs.GetString(progressKey);

            this.Log($"Game save {data}");
        }

        private void LoadProgress()
        {
            var data = PlayerPrefs.GetString(progressKey);
            _playerProgress =  PlayerPrefs.GetString(progressKey)?.ToDeserialized<SavedData>();
            _playerProgress ??= new SavedData()
            {
                AudioVolume = new AudioVolumeData()
                {
                    Effects = 1,
                    Music = 1,
                    IsEnabled = true
                }
            };
            foreach (var progressReader in _progressReader)
            {
                progressReader.LoadProgress(_playerProgress);
            }
        }
    }
}