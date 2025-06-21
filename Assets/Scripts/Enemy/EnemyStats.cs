using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStat")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public float health;
    public float speed;
    public float damage;
    public float attackCooldown;
    // Add more stats as needed
}