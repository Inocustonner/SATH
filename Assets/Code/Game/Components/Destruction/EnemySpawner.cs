using System;
using System.Linq;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Pools;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class EnemySpawner: MonoBehaviour, IGameInitListener, IPartTickListener,IRestarable
    {
        [Header("Components")]
        [SerializeField] private Transform _player;
        [SerializeField] private MonoPool<Enemy> _monoPool;
       
        [Header("Static data")]
        [SerializeField] private Vector3 _startPosition = new(0, 7.5f,0);
        private DestructionConfig _destructionConfig;
        
        [Header("Dinamyc data")]
        [SerializeField] private int _enemyStage;
        private bool _isEnd;

        public event Action<int> OnGetDamage; 
        public event Action OnEndWaves;

        public void GameInit()
        {
            _destructionConfig = Container.Instance.FindConfig<DestructionConfig>();
        }

        public void PartTick()
        {
            var all = _monoPool.GetAllEnabled();

            foreach (var enemy in all)
            {
                enemy.Move();
            }
            
            if (_enemyStage >= _destructionConfig.EnemyStages.Length)
            {
                return;
            }
            
            if (!_monoPool.GetAllEnabled().Any())
            {
                var currentStageData = _destructionConfig.EnemyStages[_enemyStage];
                int centerX = currentStageData.EnemyMatrix.Matrix[0].Array.Length / 2;

                for (int y = 0; y < currentStageData.EnemyMatrix.Matrix.Length; y++)
                {
                    var line = currentStageData.EnemyMatrix.Matrix[y];
                    for (int x = 0; x < line.Array.Length ; x++)
                    {
                        if (!line.Array[x])
                        {
                            continue;
                        }

                        var dis = _destructionConfig.DistanceBetweenEnemy;
                        var offsetX = (x - centerX) * dis.x;  
                        var pos = _startPosition + new Vector3(offsetX, -y * dis.y, 0);
                        var enemy = _monoPool.GetNext(GetInitParams());
                        enemy.transform.position = pos;
                        enemy.OnDeath += () => _monoPool.Disable(enemy);
                        enemy.OnTakeDamage += () =>
                        {
                            _monoPool.Disable(enemy);
                            OnGetDamage?.Invoke(_destructionConfig.EnemyDamage);
                        };
                        enemy.SetStageParam(pos,currentStageData.ColumnSpeed, currentStageData.Acceleration);
                        enemy.transform.position = pos;
                    }
                }

                _enemyStage++;
            }
                /*if (_enemyStage == _destructionConfig.EnemyStages.Length)
                {
                    OnEndWaves?.Invoke();
                }*/
        }

        public void Restart()
        {
            _enemyStage = 0;
            _isEnd = false;
        }

        private object[] GetInitParams()
        {
            return new object[]
            {
                _destructionConfig.GameSpeed,
                _destructionConfig.DefaultDistance,
                _destructionConfig.EnemyHP,
                _player
            };
        }
    }
}