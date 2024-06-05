using System.Collections;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.GameParts.Components;
using Code.GameParts.CustomActions.Actions;
using UnityEngine;

namespace Code.GameParts
{
    public class GamePart_1_School : GamePart, IGameStartListener, IGameExitListener
    {
        public override GamePartName GamePartName => GamePartName.Part_1__school;
        public int AttemptNumber { get; private set; }
        public bool IsWin{ get; private set; }
        
        [Header("Components")] 
        [SerializeField] private CollisionObserver _winZone;
        [SerializeField] private CollisionObserver _deathZone_restart;
        [SerializeField] private CollisionObserver _deathZone_lose;
        [SerializeField] private ReplicasAction _loseReplicas;

        [Header("Restrart params")] 
        [SerializeField] private float _restartDelay;

        [Header("Dinamyc data")] 
        private Coroutine _restartCoroutine;
        
        public void GameStart()
        {
            SubscribeToEvents(true);
        }

        public void GameExit()
        {
            SubscribeToEvents(false);
        }

        public override void Restart()
        {
            base.Restart();
            
            AttemptNumber = 0;
            IsWin = false;
            
            InvokeUpdateDataEvent();
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _deathZone_restart.OnEnter += OnEnterDeathZone_Restart;
                _deathZone_lose.OnEnter += OnEnterDeathZone_Lose;
                _winZone.OnEnter += OnEnterWinZone;
            }
            else
            {
                _deathZone_restart.OnEnter -= OnEnterDeathZone_Restart;
                _deathZone_lose.OnEnter -= OnEnterDeathZone_Lose;
                _winZone.OnEnter -= OnEnterWinZone;
            }
        }

        private void OnEnterWinZone(GameObject obj)
        {
            IsWin = true;
            InvokeUpdateDataEvent();
        }

        private void OnEnterDeathZone_Restart(GameObject obj)
        {
            AttemptNumber++;
            InvokeUpdateDataEvent();
            if (AttemptNumber == 3)
            {
                _deathZone_restart.gameObject.SetActive(false);
                _deathZone_lose.gameObject.SetActive(true);
            }
        }

        private void OnEnterDeathZone_Lose(GameObject obj)
        {
            if (_restartCoroutine != null)
            {
                StopCoroutine(_restartCoroutine);
            }
            _restartCoroutine =  StartCoroutine(TryRestartWithDelay());
        }

        private IEnumerator TryRestartWithDelay()
        {
            _loseReplicas.StartAction();
            yield return new WaitUntil(() => !_loseReplicas.InProgress);
            yield return new WaitForSeconds(_restartDelay);
            InvokeTryRestartEvent();
        }
    }
}