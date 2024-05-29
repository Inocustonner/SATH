using UnityEngine;

namespace Code.CustomActions.Actions
{
    public abstract class CustomAction : MonoBehaviour
    {
        public abstract void StartAction();

        public virtual void StopAction() { }
    }
}