﻿using System;
using Code.Data.Values.RangeFloat;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "FactoryConfig", menuName = "Config/FactoryConfig")]
    public class FactoryConfig : ScriptableObject
    {
        public Color[] WorkerColors;
        public FactoryWorkerData[] WorkersData = new FactoryWorkerData[2];

        [Header("Back to factory")] 
        public float DarkCurtainSpeed = 0.01f;

        private void OnValidate()
        {
            for (int i = 0; i < WorkerColors.Length; i++)
            {
                WorkerColors[i].a = 1;
            }
        }
    }

    [Serializable]
    public struct FactoryWorkerData
    {
        [MinMaxRangeFloat(2,50)] public RangedFloat SpawnCooldownSec;
        [MinMaxRangeFloat(0,5)] public RangedFloat SwitchSpeedCooldownSec;
        [MinMaxRangeFloat(1,5)] public RangedFloat WorkerSpeed;
    }
}