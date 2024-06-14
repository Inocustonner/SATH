using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Factory
{
    public class PushListener: MonoBehaviour, IPartTickListener
    {
        [SerializeField] private Rigidbody2D _stone;
        [SerializeField] private float _pushTime;
        public bool IsPushed { get; private set; }
        
        public void PartTick()
        {
            if (_stone.velocity.magnitude > 0)
            {
                _pushTime += Time.deltaTime;
                if (!IsPushed)
                {
                    IsPushed = true;
                }
            }
            else if(IsPushed)
            {
                IsPushed = false;
            }
        }
        
        public float GetPushTime()
        {
            return _pushTime;
        }
    }
}