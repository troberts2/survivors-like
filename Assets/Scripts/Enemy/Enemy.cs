using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int TakeHit = Animator.StringToHash("take hit");
    private static readonly int Death = Animator.StringToHash("death");
    private static readonly int Attack = Animator.StringToHash("attack");
    private bool isFacingRight = true;

    private bool isAttacking = false;
    [SerializeField] private float attackAnimDuration;
    [SerializeField] private float attackDistance = 0.3f;
    private float currentAttackCooldown;

    private bool isStunned = false;
    [SerializeField] private float stunAnimDuration;

    private bool isDead = false;
    [SerializeField] private float deadAnimDuration;
    private float lockedTill;
    private int currentState;

    private Transform playerLoc;
    private EnemyManager em;
    private float currentHealth;

    [SerializeField] private EnemyStats stats;
    private PlayerMovement pm;

    public EnemyStats Stats { get => stats; set => stats = value; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// for initalizing enemy called by enemyManager when spawned in
    /// </summary>
    public virtual void Initialize(Transform playerLocation, EnemyManager enemyManager, PlayerMovement playerMovement)
    {
        playerLoc = playerLocation;
        em = enemyManager;
        currentHealth = Stats.health;
        pm = playerMovement;
    }

    /// <summary>
    /// update animation state and do cooldowns
    /// </summary>
    public virtual void Update()
    {
        if (currentAttackCooldown >= 0) currentAttackCooldown -= Time.deltaTime;

        //animator state
        var state = GetState();

        if (state != currentState)
        {
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }
    }

    //handle whether to move or attack
    public virtual void FixedUpdate()
    {
        if (playerLoc != null && !isAttacking && !isStunned && !isDead)
        {
            Vector3 playerPos = playerLoc.position;
            if (Vector2.Distance(playerPos, transform.position) <= attackDistance)
            {
                AttackPlayer(null);
            }
            else
            {
                Vector2 moveDir = (playerPos - transform.position).normalized;
                rb.velocity = moveDir * Stats.speed;
                if (rb.velocity.x > 0 && !isFacingRight)
                {
                    isFacingRight = true;
                    transform.Rotate(0, 180, 0);
                }
                else if (rb.velocity.x < 0 && isFacingRight)
                {
                    isFacingRight = false;
                    transform.Rotate(0, -180, 0);
                }
            }
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

    /// <summary>
    /// handle triggers, just attacks for now
    /// </summary>
    /// <param name="collider"></param>
    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerBullet"))
        {
            ChangeHealth(-pm.BaseDamage);
        }
    }

    /// <summary>
    /// called when colliding with player or when close to player
    /// </summary>
    /// <param name="ph"></param>
    public virtual void AttackPlayer(PlayerHealth ph)
    {
        //dont attack if on cd
        if (currentAttackCooldown > 0) { return; }

        //attack and set attack cd
        currentAttackCooldown = Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke(nameof(StopAttacking), attackAnimDuration - .1f);
        //make stop attacking better TODO
    }

    /// <summary>
    /// get animations state
    /// </summary>
    /// <returns></returns>
    private int GetState()
    {
        if (Time.time < lockedTill) return currentState;

        //priorities
        if (isStunned) return LockState(TakeHit, stunAnimDuration);
        if (isDead) return LockState(Death, deadAnimDuration);
        if (isAttacking) return LockState(Attack, attackAnimDuration);
        if (rb.velocity.magnitude > 0) return Run;
        else return Idle;

        int LockState(int s, float t)
        {
            lockedTill = Time.time + t;
            return s;
        }
    }

    /// <summary>
    /// stops attack state and unrestrains
    /// </summary>
    private void StopAttacking()
    {
        isAttacking = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// stops stun state and unrestrains
    /// </summary>
    private void StopStun()
    {
        isStunned = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// stops death state, spawns xp, and destroys gameobject
    /// </summary>
    public virtual void Die()
    {
        isDead = false;
        Instantiate(Stats.xpDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    /// <summary>
    /// changes health and calls damage numbers from enemyManager
    /// </summary>
    /// <param name="damage"></param>
    public virtual void ChangeHealth(float damage)
    {
        currentHealth += damage;

        em.EnemyDamageTaken(damage, transform.position);
        isStunned = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke(nameof(StopStun), stunAnimDuration);

        if (currentHealth <= 0)
        {
            isDead = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke(nameof(Die), deadAnimDuration);
        }
    }
}
