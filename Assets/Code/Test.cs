using Code.Data.Interfaces;
using Code.Infrastructure.GameLoop;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour, IGameTickListener
    {

        [SerializeField] private float _timeScale = 1;
        
        public void GameTick()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            Time.timeScale = _timeScale;
        }
    }
}