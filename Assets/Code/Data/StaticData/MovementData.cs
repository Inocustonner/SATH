using System;
using UnityEngine;

namespace Code.Data.StaticData
{
    [Serializable]
    public struct MovementData
    {
        [SerializeField, Range(0f, 20f)] [Tooltip("Maximum movement speed")]
        public float MaxSpeed;

        [SerializeField, Range(0f, 100f)] [Tooltip("How fast to reach max speed")]
        public float MaxAcceleration;

        [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop after letting go")]
        public float MaxDeceleration;

        [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop when changing direction")]
        public float MaxTurnSpeed ;

        [SerializeField] [Tooltip("Friction to apply against movement on stick")]
        public float Friction;
    }
}