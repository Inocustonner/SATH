using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class BossController : MonoBehaviour, IGameInitListener
    {
        public Health Health { get; private set; }

        public void GameInit()
        {
            
        }
    }
}

    