using System;
using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public abstract class Bullet: MonoBehaviour, IPoolEntity
    {
        public abstract void Move();
        public abstract void InitEntity(params object[] parameters);
        public abstract void EnableEntity();
        public abstract void DisableEntity();

        public event Action<GameObject> OnTouched;

        protected void InvokeTouchedEvent(GameObject obj)
        {
            OnTouched?.Invoke(obj);
        }
    }
}