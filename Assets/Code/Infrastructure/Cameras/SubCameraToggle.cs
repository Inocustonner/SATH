using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.Components
{
    public class SubCameraToggle: MonoBehaviour, IGameInitListener,IPartStartListener, IPartExitListener
    {
        [SerializeField] private Camera _subCamera;
        private Camera _mainCamera;

        private CameraService _cameraService;
        public void GameInit()
        {
            _cameraService = Container.Instance.FindService<CameraService>();
        }

        public void PartStart()
        {
            _mainCamera ??= Camera.main;
            _subCamera.gameObject.SetActive(true);
            _mainCamera.gameObject.SetActive(false);
            _cameraService.SetCurrentCamera(_subCamera, isMovement: true);
        }

        public void PartExit()
        {
            if(_mainCamera != null)
            {
                _mainCamera.gameObject.SetActive(true);
            }
            _subCamera.gameObject.SetActive(false);
            _cameraService.SetCurrentCamera(_mainCamera, isMovement: false);
        }

        private void OnDestroy()
        {
            _mainCamera = null;
            _subCamera = null;
        }
    }
}