using Code.Data.Interfaces;
using Code.Game.Components;
using Code.Utils;
using UnityEngine;

namespace Code.Game.CustomActions.Actions
{
    public class MoveToPointAction: CustomAction, IRestarable
    {
        [Header("Components")]
        [SerializeField] private ObjectMover _mover;
        [SerializeField] private Collider2D[] _disableColliders;

        [Header("Params")] 
        [SerializeField] private Vector2 _target;

        [Header("Dynamic data")] 
        private Coroutine _coroutine;
        
        public void Restart()
        {
            TryStopCoroutine();
        }

        public override void StartAction()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            TryStopCoroutine();
            InvokeStartActionEvent();
            SetComponentsEnable(false);
       
            this.Log($"start");
            _coroutine = StartCoroutine(_mover.Move(_target, onCompeted: () =>
            {
                SetComponentsEnable(true);
                InvokeEndActionEvent();
            }));
        }

        private void TryStopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        private void SetComponentsEnable(bool isEnabled)
        {
            foreach (var collider in _disableColliders)
            {
                collider.enabled = isEnabled;
            }    
        }

        private void OnDrawGizmosSelected()
        {
            var color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(_target, 0.2f);
        }
    }
}