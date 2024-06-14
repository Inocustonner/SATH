using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components
{
    public class CameraService: IService, IGameInitListener
    {
        public Camera CurrentCamera { get; private set; }

        public bool IsMovement{ get; private set; }

        public void GameInit()
        {
            CurrentCamera = Camera.main;
        }

        public void SetCurrentCamera(Camera camera, bool isMovement)
        {
            CurrentCamera = camera;
            IsMovement = isMovement;
        }
    }
}