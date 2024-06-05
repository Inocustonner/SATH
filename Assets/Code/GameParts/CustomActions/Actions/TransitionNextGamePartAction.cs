using System;
using Code.Data.StaticData;
using UnityEngine;

namespace Code.GameParts.CustomActions.Actions
{
    public class TransitionNextGamePartAction : CustomAction
    {
        [SerializeField] private NextGamePartData[] _nextGameParts;
        public event Action<NextGamePartData[]> OnTryStart;

        public override void StartAction()
        {
            OnTryStart?.Invoke(_nextGameParts);
        }
    }
}