using Code.Data.Enums;
using UnityEngine;

namespace Code.Replicas
{
    public class GamePart : MonoBehaviour
    {
        [SerializeField] private GamePartName _gamePartName;
        public GamePartName GamePartName => _gamePartName;
        
        #region Editor

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            gameObject.name = $"{_gamePartName}";
        }
#endif
        #endregion
    }
}