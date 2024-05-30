using Code.Data.Configs;
using Code.Infrastructure.Services;
using UnityEngine;

namespace Code.Replicas.Scripts
{
    public sealed class ReplicaTest : MonoBehaviour
    {
        [SerializeField] private Lan _lan;
        [SerializeField] private ReplicaConfig _config;

        private GameConditionService _conditionService = new();
        private ReplicaConverter _replicaConverter;


        /*private void Start()
        {
            _replica = new Replica(_config);
            PrintDialog();
        }

        [ContextMenu("Я хочу умереть")]
        private void PrintDialog()
        {
            Debug.Log("----");
            if (_replica.TryGetAcceleratedText(_lan, out var replicaMessage))
            {
                Debug.Log($"Message: {replicaMessage}");
                if (_replica.TryGetCurrentConditions(out var gameConditions))
                {
                    for (var index = 0; index < gameConditions.Length; index++)
                    {
                        var gameCondition = gameConditions[index];
                        if (_conditionService.GetValue(gameCondition))
                        {
                            Debug.Log($"Condition is true: {gameCondition}");
                            _replica.MoveNext(index);
                        }
                    }
                }
                else
                {
                Debug.Log($"кондишены проебаны)");
                    
                }
            }
            else
            {
                Debug.Log($"Suck my dick bitch");
            }


            Debug.Log("----");
        }*/
    }
}