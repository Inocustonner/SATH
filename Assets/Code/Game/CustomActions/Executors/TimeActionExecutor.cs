using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.CustomActions.Executors
{
    public class TimeActionExecutor: MonoBehaviour,IPartStartListener, IPartTickListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction _action;

        [Header("Param")]
        [SerializeField] private float _cooldownSec = 1;

        [Header("Dinamyc data")]
        private float _currentCooldown;
        
        public void PartStart()
        {
            _currentCooldown = 0;
        }

        public void PartTick()
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