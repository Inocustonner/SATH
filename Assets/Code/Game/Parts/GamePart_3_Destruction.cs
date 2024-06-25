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
        [SerializeField] private Health _playerHealth;
        [SerializeField] private EnemySpawner _enemySpawner;

        public void GameInit()
        {
            _playerHealth.Set(Container.Instance.FindConfig<DestructionConfig>().PlayerHP);    
        }

        public void PartStart()
        {
            _playerHealth.Reset();
            SubscribeToEvents(true);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        public override void Restart()
        {
            _playerHealth.Reset();
            base.Restart();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _enemySpawner.OnGetDamage += OnGetDamage;
                _enemySpawner.OnEndWaves += OnEndWaves;
            }
            else
            {
                _enemySpawner.OnGetDamage -= OnGetDamage;
                _enemySpawner.OnEndWaves -= OnEndWaves;
            }
        }

        private void OnGetDamage(int damage)
        {
            _playerHealth.GetDamage(damage);
        }

        private void OnEndWaves()
        {   
            //todo boos enter
        }
    }
}