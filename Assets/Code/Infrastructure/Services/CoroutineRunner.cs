using System;
using System.Collections;
using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class CoroutineRunner : MonoBehaviour, IService
    {
        public void OnDisable()
        {
            StopAllCoroutines();
        }

        public Coroutine StartRoutine(IEnumerator coroutine)
        {
            return coroutine == null ? null : StartCoroutine(coroutine);
        }

        public Coroutine StartActionWithDelay(Action action, float delay)
        {
            return StartCoroutine(StartActionWithDelayRoutine(action,delay));
        }

        public void StopRoutine(IEnumerator coroutine)
        {
            if (!gameObject.activeInHierarchy || coroutine == null)
            {
                return;
            }
            StopCoroutine(coroutine);
        }

        public void StopRoutine(Coroutine coroutine)
        {
            if (gameObject.IsDestroyed() || coroutine == null)
            {
                return;
            }
            StopCoroutine(coroutine);
        }

        private IEnumerator StartActionWithDelayRoutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}