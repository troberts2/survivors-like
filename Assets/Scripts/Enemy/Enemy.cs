using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        None,
        Mushroom
    }
    internal EnemyType enemyType;
    internal float enemyHealth;
    internal float enemySpeed = 2.5f;
    internal float enemyDamage = 1f;
    internal float attackCooldown = 1.0f;
    internal float currentAttackCooldown;
    internal Rigidbody2D rb;
    public Transform playerLoc;
    internal EnemyManager em;

    internal EnemyStats stats;

    /// <summary>
    /// for initalizing enemy called by enemyManager when spawned in
    /// </summary>
    public virtual void Initialize(Transform playerLocation, EnemyStats stats, EnemyManager enemyManager)
    {
        rb = GetComponent<Rigidbody2D>();
        /*enemyHealth = health;
        enemySpeed = speed;
        enemyDamage = damage;*/
        /*this.attackCooldown = attackCooldown;*/
        playerLoc = playerLocation;
        em = enemyManager;
        this.stats = stats;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(currentAttackCooldown >= 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// for movement only right now
    /// </summary>
    public virtual void FixedUpdate()
    {
        if(playerLoc != null)
        {
            Vector2 moveDir = (playerLoc.position - transform.position).normalized;
            rb.velocity = moveDir * enemySpeed;
        }
    }

    /// <summary>
    /// normal collisions
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        var collider = collision.collider;

        if(collider.CompareTag("Player"))
        {
            AttackPlayer(collider.GetComponent<PlayerHealth>());
        }

        if(collider.CompareTag("PlayerBullet"))
        {
            ChangeHealth(-10);
            Destroy(collider.gameObject);
        }
    }

    /// <summary>
    /// deals damage to player, called on collision
    /// </summary>
    /// <param name="ph"></param>
    public virtual void AttackPlayer(PlayerHealth ph)
    {
        //dont attack if on cd
        if(currentAttackCooldown > 0) { return; }

        //attack and set attack cd
        currentAttackCooldown = attackCooldown;
        ph.ChangeHealth(-enemyDamage);
    }

    /// <summary>
    /// changes health and calls damage numbers from enemyManager
    /// </summary>
    /// <param name="damage"></param>
    private void ChangeHealth(float damage)
    {
        enemyHealth += damage;

        em.EnemyDamageTaken(damage, transform.position);

        if(enemyHealth <= 0)
        {
            //handle enemy destory better TODO
            Destroy(gameObject);
        }
    }
}
