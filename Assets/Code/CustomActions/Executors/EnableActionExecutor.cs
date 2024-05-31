using Code.CustomActions.Actions;
using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class EnableActionExecutor: ActionExecutor
    {
        [Header("Components")] 
        [SerializeField] private CustomAction[] _customActions;

        [Header("Components")] 
        [SerializeField] private bool _isStopActionWhenDisable = true;
        
        
        private void OnEnable()
        {
            if (IsCanInvoke())
            {
                foreach (var action in _customActions)
                {
                    action.StartAction();
                }
            }
        }

        private void OnDisable()
        {
            if (IsCanInvoke())
            {
                if (!_isCanRepeat)
                {
                    _isInvoked = true;
                }
                
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
}