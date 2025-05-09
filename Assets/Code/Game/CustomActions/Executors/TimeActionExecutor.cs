﻿using System;
using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions;
using UnityEngine;

namespace Code.Game.CustomActions.Executors
{
    public class TimeActionExecutor: MonoBehaviour,IPartTickListener
    {
        [Header("Components")]
        [SerializeField] private CustomAction _action;

        [Header("Param")]
        [SerializeField] private float _cooldownSec = 1;
        [SerializeField] private int _repeat;
        [SerializeField] private bool _startOnEnable;
        
        [Header("Dinamyc data")]
        private float _currentCooldown;
        private int _currentRepeat;

        private void OnEnable()
        {
            _currentCooldown = _startOnEnable ? _cooldownSec: 0;
            _currentRepeat = 0;
        }

        public void PartTick()
        {
            if (!gameObject.activeInHierarchy || (_repeat > 0 && _currentRepeat >= _repeat))
            {
                return;
            }

            _currentCooldown += Time.deltaTime;
            if (_currentCooldown > _cooldownSec && !_action.InProgress)
            {
                _action.StartAction();
                _currentCooldown = 0;
                _currentRepeat++;
            }
        }
    }
}