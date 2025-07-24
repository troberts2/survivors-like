using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletPattern", menuName = "Enemy/BulletPattern")]
public class EnemyBulletPattern : ScriptableObject
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;

    [Header("Cone Settings")]
    public float coneAngle = 45f;         // Total spread angle
    public int bulletsPerShot = 5;

    [Header("Firing Settings")]
    public int repeatCount = 3;
    public float delayBetweenShots = 0.5f;

    public float shootDirAngle = 0f;
    public bool targetPlayer = false;

    public float fireDelay = 0f;
}
