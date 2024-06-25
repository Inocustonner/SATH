using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class PlayerController : MonoBehaviour, IGameInitListener, IPartStartListener, IPartExitListener, IRestarable
    {
        [SerializeField] private CollisionObserver _collisionObserver;
        public Health Health { get; private set; } 
        
        public void GameInit()
        {
            Health = new Health(Container.Instance.FindConfig<DestructionConfig>().PlayerHP);    
        }

        public void PartStart()
        {
            Health.Reset();
            SubscribeToEvents(true);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        public  void Restart()
        {
            Health.Reset();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _collisionObserver.OnEnter += OnCollisionEnter;
            }
            else
            {
                _collisionObserver.OnEnter -= OnCollisionEnter;
            }
        }

        private void OnCollisionEnter(GameObject obj)
        {
            if (obj.TryGetComponent(out Enemy enemy))
            {
                Health.GetDamage(enemy.Damage);
            }
            else if(obj.TryGetComponent(out BossBullet bossBullet))
            {
                Health.GetDamage(bossBullet.Damage);
                
            }
        }

        /*p void OnGetDamage(int damage)
        {
            Health.GetDamage(damage);
        }*/
    }
}