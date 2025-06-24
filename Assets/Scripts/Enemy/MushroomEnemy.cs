using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class MushroomEnemy : Enemy
{
    [SerializeField] private Animator animator;
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
    public override void Initialize(Transform playerLocation, EnemyStats stats, EnemyManager enemyManager)
    {
        base.Initialize(playerLocation, stats, enemyManager);
        
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
            Debug.Log(state.ToString());
            currentState = state;
        }
    }

    public override void FixedUpdate()
    {
        if (playerLoc != null)
        {
            Vector3 playerPos = playerLoc.position;
            if (Vector2.Distance(playerPos, transform.position) <= attackDistance && !isAttacking) 
            {
                AttackPlayer(null);
            }
            else if(!isAttacking)
            {
                Vector2 moveDir = (playerPos - transform.position).normalized;
                rb.velocity = moveDir * enemySpeed;
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
        currentAttackCooldown = attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
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
    }
}
