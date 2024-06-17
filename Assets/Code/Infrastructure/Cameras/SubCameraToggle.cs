using Code.Data.Interfaces;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Infrastructure.Cameras
{
    public class SubCameraToggle: MonoBehaviour, IGameInitListener,IPartStartListener, IPartExitListener
    {
        [SerializeField] private Camera _subCamera;

        private CameraService _cameraService;
        public void GameInit()
        {
            _cameraService = Container.Instance.FindService<CameraService>();
        }

        public void PartStart()
        {
            _cameraService.SetCurrentCamera(_subCamera, isMovement: true);
        }

        public void PartExit()
        {
            _cameraService.SwitchToMainCamera();
        }
    }
}