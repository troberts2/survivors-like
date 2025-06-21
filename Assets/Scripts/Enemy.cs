using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private float enemyHealth;
    private float enemySpeed = 5f;
    private float enemyDamage = 1f;
    private float attackCooldown = 1.0f;
    private float currentAttackCooldown;
    private Rigidbody2D rb;
    private Transform playerLoc;
    private EnemyManager em;

    /// <summary>
    /// for initalizing enemy called by enemyManager when spawned in
    /// </summary>
    public void Initialize(float health, float speed, float damage, float attackCooldown, Transform playerLocation, EnemyManager enemyManager)
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = health;
        enemySpeed = speed;
        enemyDamage = damage;
        this.attackCooldown = attackCooldown;
        playerLoc = playerLocation;
        em = enemyManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAttackCooldown >= 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// for movement only right now
    /// </summary>
    private void FixedUpdate()
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
    private void AttackPlayer(PlayerHealth ph)
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
