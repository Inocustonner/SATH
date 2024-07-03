using System.Linq;
using Code.Data.Configs;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.Pools;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class BulletSpawner : MonoBehaviour, IGameInitListener, IPartTickListener
    {
        [Header("Components")]
        [SerializeField] private MonoPool<Bullet> _pool;

        [Header("Static data")] 
        [SerializeField] private BulletType _type;
        private BulletData _data;
        
        public void GameInit()
        {
            _data = Container.Instance.FindConfig<DestructionConfig>().Bullets.FirstOrDefault(b => b.Type == _type);
        }
        
        public void PartTick()
        {
            var allBullets = _pool.GetAllEnabled().ToArray();
            for (int i = 0; i < allBullets.Length; i++)
            {
                allBullets[i].Move();
            }
        }

        [ContextMenu("Spawn")]
        public  void Spawn(Vector3 position, Vector3 rotation)
        {
            Debug.Log("Spawning bullet at " + position);
            var bullet = _pool.GetNext(
                _data.Damage, 
                _data.Speed, 
                position, 
                rotation);
            
            bullet.OnTouched += obj => _pool.Disable(bullet);
           // bullet.transform.rotation = Quaternion.Euler(rotation);
            bullet.transform.position = position;
        }
    }
}