using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class MushroomEnemy : Enemy
{
    private Animator animator;
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int TakeHit = Animator.StringToHash("take hit");
    private static readonly int Death = Animator.StringToHash("death");
    private static readonly int Attack = Animator.StringToHash("attack");
    private bool isFacingRight = true;

    private float lockedTill;
    private int currentState;

    private bool isAttacking = false;
    [SerializeField] private float attackAnimDuration;
    [SerializeField] private float attackDistance = 0.3f;

    private bool isStunned = false;
    [SerializeField] private float stunAnimDuration;

    private bool isDead = false;
    [SerializeField] private float deadAnimDuration;
    /// <summary>
    /// for initalizing enemy called by enemyManager when spawned in
    /// </summary>
    public override void Initialize(Transform playerLocation, EnemyManager enemyManager)
    {
        base.Initialize(playerLocation, enemyManager);
        
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        animator.CrossFade(Idle, 0, 0);
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        base.Update();
        var state = GetState();

        if (state != currentState)
        {
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }
    }

    public override void FixedUpdate()
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
                rb.velocity = moveDir * stats.speed;
                if(rb.velocity.x > 0 && !isFacingRight)
                {
                    isFacingRight = true;
                    transform.Rotate(0, 180, 0);
                }
                else if(rb.velocity.x < 0 && isFacingRight)
                {
                    isFacingRight = false;
                    transform.Rotate(0, -180, 0);
                }
            }
        }
    }

    public override void AttackPlayer(PlayerHealth ph)
    {
        //dont attack if on cd
        if (currentAttackCooldown > 0) { return; }

        //attack and set attack cd
        currentAttackCooldown = stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke(nameof(StopAttacking), attackAnimDuration - .1f);
        //make stop attacking better TODO
    }

    private int GetState()
    {
        if (Time.time < lockedTill) return currentState;

        //priorities
        if (isStunned) return LockState(TakeHit, stunAnimDuration);
        if(isDead) return LockState(Death, deadAnimDuration);
        if (isAttacking) return LockState(Attack, attackAnimDuration);
        if (rb.velocity.magnitude > 0) return Run;
        else return Idle;

        int LockState(int s, float t)
        {
            lockedTill = Time.time + t;
            return s;
        }
    }

    private void StopAttacking()
    {
        isAttacking = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void StopStun()
    {
        isStunned = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void DeathStuff()
    {
        isDead = false;
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var collider = collision.collider;
        if (collider.CompareTag("PlayerBullet"))
        {
            ChangeHealth(-5);
            Destroy(collider.gameObject);
        }
    }

    /// <summary>
    /// changes health and calls damage numbers from enemyManager
    /// </summary>
    /// <param name="damage"></param>
    public override void ChangeHealth(float damage)
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
            Invoke(nameof(DeathStuff), deadAnimDuration);
        }
    }
}
