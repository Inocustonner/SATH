using Code.CustomActions.Actions;
using Code.Data.Enums;
using Code.Entities;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Replicas
{
    public class GamePart_School : GamePart, IGameStartListener, IGameExitListener
    {
        public override GamePartName GamePartName => GamePartName.Part_1__school;
        public int AttemptNumber { get; private set; }
        public bool IsWin{ get; private set; }
        
        [Header("Components")] 
        [SerializeField] private CollisionObserver _deathZone;
        [SerializeField] private ReplicaNumerableAction _teacherFalseReplica;

        [Header("Restrart params")] 
        [SerializeField] private Transform _player;
        [SerializeField] private Ball _ball;
        [SerializeField] private GameObject[] _enabledObjects;
        [SerializeField] private GameObject[] _disabledObjects;
        private Vector3 _startPlayerPosition;
        private Vector3 _startBallPosition;
        public void GameStart()
        {
            _startPlayerPosition = _player.position;
            _startBallPosition = _ball.transform.position;
            SubscribeToEvents(true);
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _deathZone.OnEnter += OnEnter;
            }
            else
            {
                _deathZone.OnEnter -= OnEnter;
            }
        }

        private void OnEnter(GameObject obj)
        {
            AttemptNumber++;
            if (AttemptNumber <= 3)
            {
                _teacherFalseReplica.SetID(AttemptNumber);
                if (AttemptNumber == 3)
                {
                    InvokeTryRestartEvent();
                    
                }
            }
        }

        public override void Reset()
        {
            AttemptNumber = 0;
            _teacherFalseReplica.SetID(0);

            foreach (var enabledObject in _enabledObjects)
            {
                enabledObject.SetActive(true);
            }

            foreach (var disabledObject in _disabledObjects)
            {
                disabledObject.SetActive(false);
            }
            
            _player.position = _startPlayerPosition;
            _ball.transform.position = _startBallPosition;
            _ball.SwitchFollow(isFollow: false);
        }
        
    }
}