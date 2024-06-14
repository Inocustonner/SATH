using System.Collections;
using Code.Utils;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class DelayAction: CustomAction
    {
        [SerializeField] private float _delay;
        private Coroutine _coroutine;
        
        public override void StartAction()
        {
            TryStopCoroutine();

            InvokeStartActionEvent();

            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            _coroutine = StartCoroutine(Delay());
        }

        public override void StopAction()
        {
            TryStopCoroutine();
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_delay);
            InvokeEndActionEvent();
        }
    }
}