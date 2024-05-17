using Code.Data.StaticData;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/Character")]
    public class CharacterConfig : ScriptableObject
    {
        public MovementData Movement;
    }
}