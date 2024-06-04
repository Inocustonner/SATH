using Code.Data.Enums;
using UnityEngine;

namespace Code.Replicas
{
    public class GamePart_1_Factory: GamePart
    {
        public override GamePartName GamePartName => GamePartName.Part_1__factory;
        [SerializeField] private Camera _subCamera;

        private void OnEnable()
        {
            _subCamera.gameObject.SetActive(true);
            Camera.main.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Camera.main?.gameObject.SetActive(true);
            _subCamera.gameObject.SetActive(false);
        }

        public override void Reset()
        {
            
        }
    }
}