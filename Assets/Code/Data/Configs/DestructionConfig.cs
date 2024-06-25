using Code.Data.StaticData;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "DestructionConfig", menuName = "Config/DestructionConfig")]
    public class DestructionConfig : ScriptableObject
    {
        public Vector2 DistanceBetweenEnemy;
        public float GameSpeed = 1;
        public float DefaultDistance;
        public int BossHp, EnemyHP, PlayerHP;
        public BulletData PlayerBullet, BossBullet;
        public  DestructionEnemyData[] EnemyStages;
        public  DestructionBossData[] BossStages;
    }
}