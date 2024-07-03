using Code.Data.StaticData;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "DestructionConfig", menuName = "Config/DestructionConfig")]
    public class DestructionConfig : ScriptableObject
    {
        [Header("Base")]
        public Vector2 DistanceBetweenEnemy;
        public float GameSpeed = 1;
        public float DefaultDistance;

        [Header("Health")] 
        public int BossHp;
        public int EnemyHP; 
        public int PlayerHP;
        
        [Header("Damage")]
        public int EnemyDamage;
        public BulletData[] Bullets;
        
        [Header("Stages")]
        public DestructionEnemyData[] EnemyStages;
        public DestructionBossStageData[] BossStages;
    }
}