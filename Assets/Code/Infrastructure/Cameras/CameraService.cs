using Code.Data.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Infrastructure.Cameras
{
    public class CameraService: MonoBehaviour ,IService, IGameInitListener
    {
        [SerializeField] private Camera _mainCamera;
        public Camera CurrentCamera { get; private set; }

        public bool IsMovement{ get; private set; }

        public void GameInit()
        {
            CurrentCamera = _mainCamera;
        }

        public void SetCurrentCamera(Camera camera, bool isMovement)
        {
            CurrentCamera?.gameObject.SetActive(false);
            CurrentCamera = camera;
            CurrentCamera?.gameObject.SetActive(true);
            IsMovement = isMovement;
        }

        public void SwitchToMainCamera()
        {
            IsMovement = false;
            if (this.IsDestroyed() || CurrentCamera == _mainCamera)
            {
                return;
            }
            CurrentCamera?.gameObject.SetActive(false);
            CurrentCamera = _mainCamera;
            CurrentCamera?.gameObject.SetActive(true);
        }
    }
}