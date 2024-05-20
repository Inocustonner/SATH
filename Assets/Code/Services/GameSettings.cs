using System;
using Code.Data.Configs;
using Code.Infrastructure.DI;

namespace Code.Services
{
    public class GameSettings: IService
    {
        public Lan Language;

        public event Action OnSwitchLangueage;
    }
}