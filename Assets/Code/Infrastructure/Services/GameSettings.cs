using System;
using Code.Data.Configs;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;

namespace Code.Infrastructure.Services
{
    public class GameSettings: IService
    {
        public Lan Language;

        public event Action OnSwitchLangueage;
    }
}