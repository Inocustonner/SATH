using Code.Data.Interfaces;
using Code.Utils;
using UnityEngine;

namespace Code.GameParts.Components
{
    public class PushListener: MonoBehaviour, IPartTickListener
    {
        [SerializeField] private Rigidbody2D _stone;
        [SerializeField] private float _pushTime;

        public void PartTick()
        {
            if (_stone.velocity.magnitude > 0)
            {
                _pushTime += Time.deltaTime;
            }
        }
        
        public string GetPushTime()
        {
            return _pushTime.FormatTime();
        }
    }
}