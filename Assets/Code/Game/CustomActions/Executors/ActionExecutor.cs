using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.CustomActions.Executors
{
    public class ActionExecutor : MonoBehaviour, IRestarable
    {
        [SerializeField] protected bool _isCanRepeat = true;
        protected bool _isInvoked;
        
        protected bool IsCanInvoke()
        {
            return _isCanRepeat || (!_isCanRepeat && !_isInvoked);
        }

        public void Restart()
        {
            _isInvoked = false;
        }
    }
}