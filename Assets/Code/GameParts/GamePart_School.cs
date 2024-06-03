using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.Entities;
using Code.Infrastructure.GameLoop;
using UnityEngine;

namespace Code.Replicas
{
    public class GamePart_School : GamePart, IGameInitListener, IGameStartListener, IGameExitListener
    {
        public override GamePartName GamePartName => GamePartName.Part_1__school;
        public int AttemptNumber { get; private set; }
        public bool IsWin{ get; private set; }
        
        [Header("Components")] 
        [SerializeField] private CollisionObserver _deathZone;

        [Header("Restrart params")] 
        [SerializeField] private GameObject[] _childObjects;
        private readonly Dictionary<GameObject, bool> _childStartStates = new();
        private IRestartable[] _restartable;
        
        private Vector3 _startPlayerPosition;
        private Vector3 _startBallPosition;

        public void GameInit()
        {
            foreach (var childObject in _childObjects)
            {
                _childStartStates.Add(childObject,childObject.activeSelf);
            }

            _restartable = GetComponentsInChildren<IRestartable>(true);
        }

        public void GameStart()
        {
            SubscribeToEvents(true);
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        public override void Reset()
        {
            AttemptNumber = 0;

            foreach (var childObject in _childStartStates)  
            {
                childObject.Key.SetActive(childObject.Value);
            }

            foreach (var restartable in _restartable)
            {
                restartable.Restart();
            }
            
            InvokeUpdateDataEvent();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _deathZone.OnEnter += OnEnterDeathZone;
            }
            else
            {
                _deathZone.OnEnter -= OnEnterDeathZone;
            }
        }

        private void OnEnterDeathZone(GameObject obj)
        {
            AttemptNumber++;
            if (AttemptNumber <= 3)
            {
                if (AttemptNumber == 3)
                {
                    InvokeTryRestartEvent();
                }
            }
            InvokeUpdateDataEvent();
        }

        #region Editor

        [ContextMenu("InitChildObjects")]
        private void InitChildObjects()
        {
            _childObjects = GetComponentsInChildren<Transform>(true).Select(child => child.gameObject).ToArray();
        }

        #endregion
    }
}