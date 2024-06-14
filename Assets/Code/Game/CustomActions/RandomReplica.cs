using Code.Game.CustomActions.Actions;
using Code.Replicas.Scripts;
using UnityEngine;

namespace Code.Game.CustomActions
{
    public class RandomReplica: ReplicaAction
    {
        [SerializeField] private ReplicaConfig[] _randomReplicas;
        
        public override void StartAction()
        {
            _replicaConfig = _randomReplicas[Random.Range(0, _randomReplicas.Length)];
            base.StartAction();
        }
    }
}