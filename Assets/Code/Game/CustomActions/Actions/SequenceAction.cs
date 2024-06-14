using System;
using System.Collections;
using Code.Utils;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class SequenceAction: CustomAction
    {
        [SerializeField] private SequenceActionData[] _data;

        private Coroutine _coroutine;
        public override void StartAction()
        {
            TryStopCoroutine();
            _coroutine = StartCoroutine(StartSuperNanoActions());
            InvokeStartActionEvent();
            this.Log("start",Color.magenta);
        }

        public override void StopAction()
        {
            InvokeEndActionEvent();
            this.Log("stop",Color.magenta);
        }

        private void OnDisable()
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
        private IEnumerator StartSuperNanoActions()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var customActions = _data[i].Actions;
                for (int j = 0; j < customActions.Length; j++)
                {
                    customActions[j].StartAction();
                }
                yield return new WaitForSeconds(_data[i].Delay);
            }
            
            StopAction();
        }
    }

    [Serializable]
    public struct SequenceActionData
    {
        public CustomAction[] Actions;
        public float Delay;
    }
}