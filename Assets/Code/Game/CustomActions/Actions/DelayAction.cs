using System.Collections;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class DelayAction: CustomAction
    {
        [SerializeField] private float _delay;
        private Coroutine _coroutine;
        
        public override void StartAction()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            InvokeStartActionEvent();

            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            _coroutine = StartCoroutine(Delay());
        }

        public override void StopAction()
        {
           InvokeEndActionEvent();
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_delay);
            StopAction();
        }
    }
}