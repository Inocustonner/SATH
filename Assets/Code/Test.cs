using Code.Data.Interfaces;
using Code.Infrastructure.GameLoop;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour, IGameTickListener
    {
        public void GameTick()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            this.Log("tick");
        }
    }
}