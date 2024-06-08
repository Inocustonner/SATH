using Code.Data.Interfaces;
using Code.GameParts.CustomActions.Actions;
using UnityEngine;

namespace Code.GameParts.Components.FriendShip
{
    public class BarTimeController: MonoBehaviour, IPartStartListener, IPartExitListener, IRestarable
    {
        [Header("Time")]
        [SerializeField] private Vector2Int _defaultTime;
        [SerializeField] private Vector2Int _purpleTime;
        [SerializeField] private Vector2Int _blueTime;

        [Header("Components")]
        [SerializeField] private BarClocks _barClocks;
        [SerializeField] private CustomAction _purpleAction;
        [SerializeField] private CustomAction _blueAction;

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
                _blueAction.OnStart += OnStartBlueAction;
                _purpleAction.OnEnd += OnEndPurpleAction;
            }
            else
            {
                _blueAction.OnStart -= OnStartBlueAction;
                _purpleAction.OnEnd -= OnEndPurpleAction;
            }
        }

        private void OnStartBlueAction()
        {
            if (_isSpeakWithPurple)
            {
                return;
            }

            _barClocks.SetTime(_blueTime.x, _blueTime.y,duration: 2);
        }

        private void OnEndPurpleAction()
        {
            _isSpeakWithPurple = true;
            _barClocks.SetTime(_purpleTime.x, _purpleTime.y,duration: 2);
        }
    }
}