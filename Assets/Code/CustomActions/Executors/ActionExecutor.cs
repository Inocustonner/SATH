using UnityEngine;

namespace Code.CustomActions.Executors
{
    public class ActionExecutor : MonoBehaviour
    {
        [SerializeField] protected bool _isCanRepeat = true;
        protected bool _isInvoked;
        
        protected bool IsCanInvoke()
        {
            return _isCanRepeat || (!_isCanRepeat && !_isInvoked);
        }
    }
}