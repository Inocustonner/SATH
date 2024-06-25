using System.Linq;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Pools;
using Code.Infrastructure.Services;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class PlayerBulletSpawner: MonoBehaviour, IGameInitListener, IPartStartListener,IPartTickListener,IPartExitListener
    {
        [Header("Components")] 
        [SerializeField] private Transform _player;
        [SerializeField] private MonoPool<PlayerBullet> _pool;
       
        [Header("Services")]
        private InputService _inputService;
        
        [Header("Static data")]
        [SerializeField] private Vector3 _spawnOffset;
        private DestructionConfig _destructionConfig;


        public void GameInit()
        {
            _inputService = Container.Instance.FindService<InputService>();
            
            _destructionConfig = Container.Instance.FindConfig<DestructionConfig>();
        }

        public void PartStart()
        {
            SubscribeToEvents(true);   
        }

        public void PartTick()
        {
            var allBullets = _pool.GetAllEnabled().ToArray();
            for (int i = 0; i < allBullets.Length; i++)
            {
                allBullets[i].Move();
            }
        }

        public void PartExit()
        {
            SubscribeToEvents(false);   
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _inputService.OnPressInteractionKey += OnPressInteractionKey;
            }
            else
            {
                _inputService.OnPressInteractionKey -= OnPressInteractionKey;
            }
        }

        private void OnPressInteractionKey()
        {
            this.Log("Press ");
            var bullet = _pool.GetNext(
                _destructionConfig.PlayerBullet.Damage, 
                _destructionConfig.PlayerBullet.Speed);
            
            bullet.OnTouched += () => _pool.Disable(bullet);

            bullet.transform.position = _player.transform.position + _spawnOffset;
        }
    }
}