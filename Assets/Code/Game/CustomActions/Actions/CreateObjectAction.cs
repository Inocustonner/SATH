using UnityEngine;

namespace Code.Game.CustomActions.Actions.Single
{
    public class CreateObjectAction: CustomAction
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _spawnOffset;
        
        public override void StartAction()
        {
            InvokeStartActionEvent();
            _gameObject.transform.position = _player.position + _spawnOffset;
            _gameObject.SetActive(true);    
            InvokeEndActionEvent();
        }
    }
}