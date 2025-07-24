using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/EnemyStat")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public float health;
    public float speed;
    public float damage;
    public float attackCooldown;
    public enum EnemyType
    {
        None,
        Mushroom,
        FlyingEye,
        Goblin,
        Skeleton
    }
    public EnemyType enemyType;
    public GameObject xpDrop;
}