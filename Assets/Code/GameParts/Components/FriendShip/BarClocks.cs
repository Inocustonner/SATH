using UnityEngine;

namespace Code.GameParts.Components.FriendShip
{
    public class BarClocks: MonoBehaviour
    {
        [SerializeField] private ClocksArrow[] _clocksArrow;

        public void SetTime(int hour,int minutes, int duration)
        {
            for (int i = 0; i < _clocksArrow.Length; i++)
            {
                StartCoroutine(_clocksArrow[i].SetTimeCoroutine(hour, minutes, duration));   
            }
        }

        public void SetTime(int hour, int minutes)
        {
            for (int i = 0; i < _clocksArrow.Length; i++)
            {
                _clocksArrow[i].SetTime(hour,minutes);
            }
        }
    }
}