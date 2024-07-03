using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class PlayerAttack: Attack, IGameInitListener, IPartStartListener, IPartExitListener
    {
        [SerializeField] private Vector3 _spawnOffset;
        [SerializeField] private BulletSpawner _bulletSpawner;
        
        [Header("Services")]
        private InputService _inputService;

        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
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
                _inputService.OnPressInteractionKey += StartAttack;
            }
            else
            {
                _inputService.OnPressInteractionKey -= StartAttack;
            }
        }
        
        protected override void StartAttack()
        {
            _bulletSpawner.Spawn(transform.position + _spawnOffset, Vector3.zero);
        }
    }
}