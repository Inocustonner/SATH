using System;
using System.Collections;
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
        
        public IEnumerator SetTimeCoroutine(int hour, int minutes, float duration)
        {
            float startMinAngle = _minArrow.localRotation.eulerAngles.z;
            float startHourAngle = _hourArrow.localRotation.eulerAngles.z;

            float minuteAngle = minutes * MIN_ANGLE;
            float hourAngle = (hour % 12) * HOUR_ANGLE + (minutes / 2f);

            float endMinAngle = -minuteAngle;
            float endHourAngle = -hourAngle;

            while (endMinAngle <= startMinAngle)
            {
                endMinAngle += 360f;
            }

            while (endHourAngle <= startHourAngle)
            {
                endHourAngle += 360f;
            }

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                float currentMinAngle = Mathf.Lerp(startMinAngle, endMinAngle - 360, t);
                float currentHourAngle = Mathf.Lerp(startHourAngle, endHourAngle - 360, t);

                _minArrow.localRotation = Quaternion.Euler(0f, 0f, currentMinAngle);
                _hourArrow.localRotation = Quaternion.Euler(0f, 0f, currentHourAngle);

                yield return null;
            }

            _minArrow.localRotation = Quaternion.Euler(0f, 0f, endMinAngle % 360f);
            _hourArrow.localRotation = Quaternion.Euler(0f, 0f, endHourAngle % 360f);
        
        }
    }
}