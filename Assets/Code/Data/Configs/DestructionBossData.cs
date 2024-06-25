using UnityEngine;

namespace Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "DestructionBossData", menuName = "Config/Destruction/BossData")]
    public class DestructionBossData: ScriptableObject
    {
        public float ChillTime = 3;
        public BoolMatrix Walls = new BoolMatrix(7,7);
    }
}