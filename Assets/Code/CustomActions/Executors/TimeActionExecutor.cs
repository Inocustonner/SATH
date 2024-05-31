using System;
using Code.CustomActions.Actions;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class TimeActionExecutor: MonoBehaviour, IGameTickListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction _action;
        
        [Header("Param")]
        [SerializeField] private float _cooldownSec = 1;

        [Header("Dinamyc data")]
        private float _currentCooldown;

        private void OnEnable()
        {
            _currentCooldown = 0;
        }

        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            _currentCooldown += Time.deltaTime;
            if (_currentCooldown > _cooldownSec && !_action.InProgress)
            {
                _action.StartAction();
                _currentCooldown = 0;
            }
        }
    }
}