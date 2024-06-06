using System;
using Code.Data.Configs;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;

namespace Code.Infrastructure.Services
{
    public class GameSettings: IService
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
    }
}