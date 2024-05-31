using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ActivateObjectsAction: CustomAction
    {
        [SerializeField] private GameObject[] _activatedObjects;
        [SerializeField] private GameObject[] _disabledObjects;
        
        public override void StartAction()
        {
            foreach (var obj in _activatedObjects)
            {
                obj.SetActive(true);
            }
            foreach (var obj in _disabledObjects)
            {
                obj.SetActive(false);
            }    
        }
    }
    
}