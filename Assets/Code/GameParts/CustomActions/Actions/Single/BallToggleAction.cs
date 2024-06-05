using Code.GameParts.Entities;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions.Single
{
    public class BallToggleAction:CustomAction
    {
        [Header("Components")]
        [SerializeField] private Ball _ball;

        [Header("Params")] 
        [SerializeField] private bool _isFollow;

        public override void StartAction()
        {
            _ball.SwitchFollow(_isFollow);
        }
    }
}