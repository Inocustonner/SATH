using Code.Data.Interfaces;
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
        
        public float GetPushTime()
        {
            return _pushTime;
        }
    }
}