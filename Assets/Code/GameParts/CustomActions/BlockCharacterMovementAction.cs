using Code.GameParts.CustomActions.Actions;
using Code.GameParts.Entities;
using UnityEngine;

namespace Code.GameParts.CustomActions
{
    public class BlockCharacterMovementAction : CustomAction
    {
        [SerializeField] private bool _isBlock;
        [SerializeField] private CharacterMovement _characterMovement;
        
        public override void StartAction()
        {
            InvokeStartActionEvent();    
            if (_isBlock)
            {
                _characterMovement.Block();
            }
            else
            {
                _characterMovement.Unblock();
            }
            InvokeEndActionEvent();
        }
    }
}