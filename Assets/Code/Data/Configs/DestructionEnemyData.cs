using UnityEngine;


[CreateAssetMenu(fileName = "DestructionBossData", menuName = "Config/Destruction/EnemyData")]
public class DestructionEnemyData: ScriptableObject
{
    public float ColumnSpeed = 1;
    public float Acceleration = 1;
    public BoolMatrix EnemyMatrix = new BoolMatrix(7, 7);
}