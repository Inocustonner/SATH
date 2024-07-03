using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class BossController : MonoBehaviour, IGameInitListener
    {
        public Health Health { get; private set; }

        
       // public event Action
        
        public void GameInit()
        {
            var config = Container.Instance.FindConfig<DestructionConfig>();
            Health = new Health(config.BossHp);
 
        }
        
        
    }
}

    