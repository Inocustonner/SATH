using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.Components.Factory
{
    public class PlayerSwitchForwardObserver: MonoBehaviour, IGameTickListener
    {
        [SerializeField] private Rigidbody2D _player;
        [SerializeField] private Transform _stone;
        [SerializeField] private float _permissibleDistance = 5;
        [SerializeField] private float _permissibleTime = 2;

        [SerializeField] private CustomAction _rightAction; 
        [SerializeField] private CustomAction _leftAction; 
        private float _currentMoveTime;

        private bool _isInvoke;
        private bool _isMoveRight;
        
        public void GameTick()
        {
            if (_player.velocity.x > 0)
            {
                if (!_isMoveRight)
                {
                    _currentMoveTime = 0;
                    _isInvoke = false;
                    _isMoveRight = true;
                }
                else if(!_isInvoke 
                        && Vector3.Distance(_player.transform.position, _stone.position) >= _permissibleDistance)
                {
                    _currentMoveTime += Time.deltaTime;

                    if (_currentMoveTime >= _permissibleTime)
                    {
                        _rightAction.StartAction();
                        _isInvoke = true;
                    }
                }
            }
            else if(_player.velocity.x < 0)
            {
                if (_isMoveRight)
                {
                    _currentMoveTime = 0;
                    _isInvoke = false;
                    _isMoveRight = false;
                }
                else if(!_isInvoke)
                {
                    _currentMoveTime += Time.deltaTime;

                    if (_currentMoveTime >= _permissibleTime)
                    {
                        _leftAction.StartAction();
                        _isInvoke = true;
                    }
                }
            }
        }
    }
}