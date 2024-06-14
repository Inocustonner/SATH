using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components
{
    public class PositionResetter: MonoBehaviour, IGameStartListener, IRestarable
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