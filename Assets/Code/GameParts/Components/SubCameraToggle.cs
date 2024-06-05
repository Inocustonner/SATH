using Code.Data.Interfaces;
using Code.Utils;
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
            _mainCamera ??= Camera.main;
            _mainCamera.gameObject.SetActive(true);
            _subCamera.gameObject.SetActive(false);
        }
    }
}