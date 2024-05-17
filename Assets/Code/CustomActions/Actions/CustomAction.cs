using UnityEngine;

namespace Code.CustomActions
{
    public abstract class CustomAction : MonoBehaviour
    {
        public abstract void StartAction();
        public abstract void StopAction();
    }
}