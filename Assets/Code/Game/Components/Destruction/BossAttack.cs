using System;
using System.Collections;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class BossAttack : Attack, IGameInitListener, IRestarable, IPartTickListener
    {
        [SerializeField] private BulletSpawner _bulletSpawner;
        [SerializeField] private int _bulletsShootCount;
        [SerializeField] private Vector3 _spawnOffset;
        private DestructionBossStageData[] _stages;
        private DestructionBossStageData _stageData;

        private int _stageID;
        private Coroutine _coroutine;
        public void GameInit()
        {
            var config = Container.Instance.FindConfig<DestructionConfig>();
            _stages = config.BossStages;
        }

        protected override void StartAttack()
        {
            _stageData = _stages[_stageID];
            _bulletSpawner.Spawn(transform.position + _spawnOffset, Vector3.zero);
        }

        public void Restart()
        {
            _stageID = 0;
        }

        public void PartTick()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Q pressed, spawning bullet");
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }

                _coroutine = StartCoroutine(SpawnQueueBullet());
            }
        }

        private IEnumerator SpawnQueueBullet()
        {
            var period = new WaitForSeconds(0.13f);
            for (int i = 0; i < _bulletsShootCount; i++)
            {
                _bulletSpawner.Spawn(transform.position + _spawnOffset, Vector3.down);
                yield return period;
            }
        }
        private void OnDrawGizmosSelected()
        {
            var color = Color.red;
            color.a = 0.2f;
            Gizmos.color = color; 
            Gizmos.DrawSphere(transform.position +_spawnOffset, 1f);
        }
    }
}