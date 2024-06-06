using Code.Data.Interfaces;
using UnityEngine;

namespace Code.GameParts.Components.FriendShip
{
    public class BarClocks: MonoBehaviour, IRestarable
    {
        [SerializeField] private ClocksArrow[] _clocksArrow;
        [SerializeField] private Vector2Int _defaultTime;
        
        [ContextMenu("SetDefaultTime")]
        public void Restart()
        {
            SetDefaultTime();
        }

        public void SetTime(int hour,int minutes)
        {
            for (int i = 0; i < _clocksArrow.Length; i++)
            {
                _clocksArrow[i].SetTime(hour, minutes);   
            }
        }
        
        private void SetDefaultTime()
        {
            for (int i = 0; i < _clocksArrow.Length; i++)
            {
                _clocksArrow[i].SetTime(_defaultTime.x, _defaultTime.y);
            }
        }
    }
}