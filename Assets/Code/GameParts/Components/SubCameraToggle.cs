using Code.Data.Interfaces;
using UnityEngine;

namespace Code.GameParts.Components
{
    public class SubCameraToggle: MonoBehaviour, IPartStartListener, IPartExitListener
    {
        [SerializeField] private Camera _subCamera;
        private Camera _mainCamera;
        
    
        public void PartStart()
        {
            _mainCamera ??= Camera.main;
            _subCamera.gameObject.SetActive(true);
            _mainCamera.gameObject.SetActive(false);
        }

        public void PartExit()
        {
        
            if(_mainCamera != null)
            {
                _mainCamera.gameObject.SetActive(true);
            }
            _subCamera.gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            _mainCamera = null;
            _subCamera = null;
        }
    }
}