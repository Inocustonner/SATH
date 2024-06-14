using Code.Game.Components;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.CustomActions
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