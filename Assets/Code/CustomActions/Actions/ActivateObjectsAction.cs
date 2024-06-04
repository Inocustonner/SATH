using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ActivateObjectsAction: CustomAction
    {
        [SerializeField] private GameObject[] _activatedObjects;
        [SerializeField] private GameObject[] _disabledObjects;
        [SerializeField] private bool _isStopped;
        
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

        public override void StopAction()
        {
            if (_isStopped)
            {
                foreach (var obj in _activatedObjects)
                {
                    obj.SetActive(false);
                }
                foreach (var obj in _disabledObjects)
                {
                    obj.SetActive(true);
                }    
            }
        }
    }
    
}