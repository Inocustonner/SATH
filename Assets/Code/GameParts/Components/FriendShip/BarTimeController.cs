using Code.Data.Interfaces;
using Code.GameParts.CustomActions.Actions;
using UnityEngine;

namespace Code.GameParts.Components.FriendShip
{
    public class BarTimeController: MonoBehaviour, IPartStartListener, IPartExitListener, IRestarable
    {
        [Header("Time")]
        [SerializeField] private Vector2Int _defaultTime;
        [SerializeField] private Vector2Int _endPurpleTime;
        [SerializeField] private Vector2Int _endTime;

        [Header("Components")]
        [SerializeField] private BarClocks _barClocks;
        [SerializeField] private ReplicaAction _purpleReplica;
        [SerializeField] private CollisionObserver _timeTrigger;

        private bool _isSpeakWithPurple;
        public void PartStart()
        {
            SubscribeToEvents(true);
            _barClocks.SetTime(_defaultTime.x,_defaultTime.y);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        public void Restart()
        {
            _isSpeakWithPurple = false;
            _barClocks.SetTime(_defaultTime.x,_defaultTime.y);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _timeTrigger.OnExit += OnEnterTimeTrigger;
                _purpleReplica.OnEnd += OnEndPurpleReplica;
            }
            else
            {
                _timeTrigger.OnExit -= OnEnterTimeTrigger;
                _purpleReplica.OnEnd -= OnEndPurpleReplica;
            }
        }

        private void OnEndPurpleReplica()
        {
            _isSpeakWithPurple = true;
        }

        private void OnEnterTimeTrigger(GameObject _)
        {
            if (_isSpeakWithPurple)
            {
                _barClocks.SetTime(_endPurpleTime.x, _endPurpleTime.y,duration: 2);
            }
            else
            {
                _barClocks.SetTime(_endTime.x, _endTime.y,duration: 2);
            }
        }
    }
}