using Code.Data.Configs;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Game.Components.Destruction;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.Parts
{
    public class GamePart_3_Destruction : GamePart,IGameInitListener, IPartStartListener, IPartExitListener
    {
        public override GamePartName GamePartName => GamePartName.Part_3__endeavours_destruction;
        [SerializeField] private PlayerController _player;
        [SerializeField] private EnemySpawner _enemySpawner;

        public void GameInit()
        {
        }

        public void PartStart()
        {
            SubscribeToEvents(true);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }


        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _enemySpawner.OnEndWaves += OnEndWaves;
            }
            else
            {
                _enemySpawner.OnEndWaves -= OnEndWaves;
            }
        }
        
        private void OnEndWaves()
        {   
            //todo boos enter
        }
    }
}