using Code.Data.Interfaces;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Entities
{
    public class PositionResetter: MonoBehaviour, IGameStartListener, IResetable
    {
        private Vector3 _startPosition;
        public void GameStart()
        {
            _startPosition = transform.position;
        }

        public void Restart()
        {
            transform.position = _startPosition;
        }
    }
}