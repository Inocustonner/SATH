using Code.Infrastructure.GameLoop;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.Services
{
    public class PushListener: MonoBehaviour, IGameTickListener
    {
        [SerializeField] private Rigidbody2D _stone;
        [SerializeField] private float _pushTime;

        public void GameTick()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

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