using System;
using UnityEngine;

namespace Code.GameParts.Components.FriendShip
{
    [Serializable]
    public class ClocksArrow 
    {
        [SerializeField] private Transform _minArrow, _hourArrow;


        private const float MIN_ANGLE = 6;
        private const float HOUR_ANGLE = 30;
        
        public void SetTime(int hour, int minutes)
        {
            var minuteAngle = minutes * MIN_ANGLE;
            var hourAngle = (hour % 12) * HOUR_ANGLE + (minutes / 2f);
            _minArrow.localRotation = Quaternion.Euler(0f, 0f, -minuteAngle);
            _hourArrow.localRotation = Quaternion.Euler(0f, 0f, -hourAngle);
        }
        
   
    }
}