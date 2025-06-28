using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    internal float currentAttackCooldown;
    internal Rigidbody2D rb;
    public Transform playerLoc;
    internal EnemyManager em;
    internal float currentHealth;

    public EnemyStats stats;
    public PlayerMovement pm;

    /// <summary>
    /// for initalizing enemy called by enemyManager when spawned in
    /// </summary>
    public virtual void Initialize(Transform playerLocation, EnemyManager enemyManager, PlayerMovement playerMovement)
    {
        rb = GetComponent<Rigidbody2D>();
        playerLoc = playerLocation;
        em = enemyManager;
        currentHealth = stats.health;
        pm = playerMovement;
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
            rb.velocity = moveDir * stats.speed;
        }
    }

    /// <summary>
    /// normal collisions
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        var collider = collision.collider;

        if(collider.CompareTag("Player"))
        {
            AttackPlayer(collider.GetComponent<PlayerHealth>());
        }

        if(collider.CompareTag("PlayerBullet"))
        {
            ChangeHealth(-pm.BaseDamage);
            Destroy(collider.gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerBullet"))
        {
            ChangeHealth(-pm.BaseDamage);
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
        currentAttackCooldown = stats.attackCooldown;
        ph.ChangeHealth(-stats.damage);
    }

    /// <summary>
    /// changes health and calls damage numbers from enemyManager
    /// </summary>
    /// <param name="damage"></param>
    public virtual void ChangeHealth(float damage)
    {
        currentHealth += damage;

        em.EnemyDamageTaken(damage, transform.position);

        Debug.Log(currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Instantiate(stats.xpDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
