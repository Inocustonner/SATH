using UnityEngine;

namespace Code.UI.Base
{
    public abstract class BaseMenuModel<T> : MonoBehaviour 
        where T : BaseMenuModel<T>
    {
        public bool IsValidating;
    }
}