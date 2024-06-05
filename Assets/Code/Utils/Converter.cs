using UnityEngine;

namespace Code.Utils
{
    public static class Converter
    {
        public static string FormatTime(this float time)
        {
            var minutes = Mathf.FloorToInt(time / 60F);
            var seconds = Mathf.FloorToInt(time % 60F);
            return $"{minutes:00}:{seconds:00}";
        }
    }
}