using System;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/Character")]
    public class CharacterConfig : ScriptableObject
    {
        public MovementData Movement;
    }

    [Serializable]
    public class MovementData
    {
        [SerializeField, Range(0f, 20f)] [Tooltip("Maximum movement speed")]
        public float MaxSpeed = 10f;

        [SerializeField, Range(0f, 100f)] [Tooltip("How fast to reach max speed")]
        public float MaxAcceleration = 52f;

        [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop after letting go")]
        public float MaxDeceleration = 52f;

        [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop when changing direction")]
        public float MaxTurnSpeed = 80f;

        [SerializeField] [Tooltip("Friction to apply against movement on stick")]
        public float Friction;
    }
}