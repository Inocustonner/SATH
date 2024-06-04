using System.Collections.Generic;
using System.Linq;
using Code.Data.Configs;
using Code.Entities;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Code.Infrastructure.Services;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.Infrastructure.Polls
{
    public class FactoryWorkersPool : MonoBehaviour, IGameInitListener ,IGameStartListener, IGameTickListener , IGameExitListener
    {
        [SerializeField] private List<RangedCooldown> _spawnCooldowns;
        [SerializeField] private MonoPool<FactoryWorker>[] _workersPools;

        private FactoryConfig _factoryConfig;

        private const int WORKERS_LINES = 2;
        
        public void GameInit()
        {
            _factoryConfig = Container.Instance.FindConfig<FactoryConfig>();
            this.Log(_factoryConfig.WorkersData.Length,Color.yellow);
            
            for (int i = 0; i < WORKERS_LINES; i++)
            {
                var data = _factoryConfig.WorkersData[i];
                _spawnCooldowns.Add(new RangedCooldown(data.SpawnCooldownSec));
            }
        }

        public void GameStart()
        {
            for (int lineIndex = 0; lineIndex < WORKERS_LINES; lineIndex++)
            {
                int line = lineIndex;
                var pool = _workersPools[lineIndex];
               
                var currentWorkers = pool.GetAll().ToArray();
                for (int workerIndex = 0; workerIndex < currentWorkers.Length; workerIndex++)
                {
                    currentWorkers[workerIndex].InitEntity(GetWorkerInitParams(lineIndex));
                }
                
                _spawnCooldowns[lineIndex].Start(isLoop:true, onCompleted: () =>
                {
                    var worker = pool.GetNext(GetWorkerInitParams(line));
                    worker.OnReached += () => { pool.Disable(worker); };
                });
            }

            for (int i = 0; i < _workersPools.Length; i++)
            {
                var workers = _workersPools[i].GetAll().ToArray();
                for (int j = 0; j < workers.Count(); j++)
                {
                    workers[i].GetTired();
                }
            }
        }

        public void GameTick()
        {
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
            this.Log(i);
            return new object[]
            {
                _factoryConfig.WorkerColors[Random.Range(0,_factoryConfig.WorkerColors.Length)],
                _factoryConfig.WorkersData[i].WorkerSpeed,
                _factoryConfig.WorkersData[i].SwitchSpeedCooldownSec
            };
        }

        public void GameExit()
        {
            for (int i = 0; i < WORKERS_LINES; i++)
            {
                _spawnCooldowns[i].Stop();
            }
        }
    }
}