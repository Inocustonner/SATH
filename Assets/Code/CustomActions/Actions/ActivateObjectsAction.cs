using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ActivateObjectsAction: CustomAction
    {
        [SerializeField] private bool _isActivate = true;
        [SerializeField] private GameObject[] _objects;
        
        public override void StartAction()
        {
            foreach (var obj in _objects)
            {
                obj.SetActive(_isActivate);
            }    
        }
    }
}