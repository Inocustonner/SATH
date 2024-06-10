using System;
using Code.Data.Configs;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.Save;

namespace Code.Infrastructure.Services
{
    public class GameSettings: IService, IProgressWriter
    {
        public Lan Language { get; private set; }

        public event Action OnSwitchLanguage;

        public void SetLanguage(Lan lan)
        {
            if (lan != Language)
            {
                Language = lan;
                OnSwitchLanguage?.Invoke();
            }
        }

        public void LoadProgress(SavedData playerProgress)
        {
            Language = playerProgress.Language;
        }

        public void UpdateProgress(SavedData playerProgress)
        {
            playerProgress.Language = Language;
        }
    }
}