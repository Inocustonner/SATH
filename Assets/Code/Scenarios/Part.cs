using Code.Data.Enums;
using UnityEngine;

namespace Code.Scenarios
{
    public class Part : MonoBehaviour
    {
        [SerializeField] private PartName _partName;
        public PartName PartName => _partName;


        #region Editor

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            gameObject.name = $"{_partName}";
        }
#endif

        #endregion
        
    }

}