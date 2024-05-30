using Code.CustomActions.Actions;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class EnableActionExecutor: MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private CustomAction[] _customActions;

        [Header("Components")] 
        [SerializeField] private bool _isStopActionWhenDisable = true;
        private void OnEnable()
        {
            foreach (var action in _customActions)
            {
                action.StartAction();
            }
        }

        private void OnDisable()
        {
            if (_isStopActionWhenDisable)
            {
                foreach (var action in _customActions)
                {
                    action.StopAction();
                }
            }
        }
    }
}