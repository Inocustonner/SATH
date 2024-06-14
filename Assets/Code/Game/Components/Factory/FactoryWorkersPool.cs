using System.Collections.Generic;
using System.Linq;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.Polls;
using Code.Infrastructure.Services;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Components.Factory
{
    public class FactoryWorkersPool : MonoBehaviour, IGameInitListener ,IPartStartListener, IPartTickListener , IPartExitListener
    {
        [SerializeField] private List<RangedCooldown> _spawnCooldowns;
        [SerializeField] private MonoPool<FactoryWorker>[] _workersPools;

        private FactoryConfig _factoryConfig;
        private EnvironmentLimiter _environmentLimiter;

        private const int WORKERS_LINES = 2;
        
        public void GameInit()
        {
            _factoryConfig = Container.Instance.FindConfig<FactoryConfig>();
            _environmentLimiter = Container.Instance.FindService<EnvironmentLimiter>();
            
            for (int i = 0; i < WORKERS_LINES; i++)
            {
                var data = _factoryConfig.WorkersData[i];
                _spawnCooldowns.Add(new RangedCooldown(data.SpawnCooldownSec));
            }
        }

        public void PartStart()
        {
            for (int lineIndex = 0; lineIndex < WORKERS_LINES; lineIndex++)
            {
                int line = lineIndex;
                var pool = _workersPools[lineIndex];
               
                var currentWorkers = pool.GetAll().ToArray();
                for (int workerIndex = 0; workerIndex < currentWorkers.Length; workerIndex++)
                {
                    currentWorkers[workerIndex].InitEntity(GetWorkerInitParams(lineIndex));
                    currentWorkers[workerIndex].GetTired();
                }
                
                _spawnCooldowns[lineIndex].Start(isLoop:true, onCompleted: () =>
                {
                    var worker = pool.GetNext(GetWorkerInitParams(line));
                    worker.OnReached += () => { pool.Disable(worker); };
                });
            }
        }

        public void PartTick()
        {
            if (!_environmentLimiter.IsUnlock)
            {
                return;
            }
            for (int poolIndex = 0; poolIndex < _workersPools.Length; poolIndex++)
            {
                var all = _workersPools[poolIndex].GetAllEnabled().ToArray();
                for (int workerIndex = 0; workerIndex < all.Length; workerIndex++)
                {
                    all[workerIndex].Work();
                }
            }    
        }

        private object[] GetWorkerInitParams(int i)
        {
            return new object[]
            {
                _factoryConfig.WorkerColors[Random.Range(0,_factoryConfig.WorkerColors.Length)],
                _factoryConfig.WorkersData[i].WorkerSpeed,
                _factoryConfig.WorkersData[i].SwitchSpeedCooldownSec
            };
        }

        public void PartExit()
        {
            for (int i = 0; i < WORKERS_LINES; i++)
            {
                _spawnCooldowns[i].Stop();
            }
        }

      
    }
}