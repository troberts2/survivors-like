using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EvilWizard : Enemy
{
    private static readonly int Attack1 = Animator.StringToHash("attack1");
    private static readonly int Attack2 = Animator.StringToHash("attack2");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int Jump = Animator.StringToHash("jump");
    [SerializeField] protected float attackAnimDuration1;
    [SerializeField] protected float attackAnimDuration2;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Image healthBar;

    /// <summary>
    /// called when colliding with player or when close to player
    /// </summary>
    /// <param name="ph"></param>
    public override void AttackPlayer(PlayerHealth ph)
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

    [SerializeField] private EnemyBulletPattern[] bulletPatterns;
    private EnemyBulletPattern currentPattern;
    private int patternIndex = 0;

    //[Header("Projectile Settings")]
    private GameObject projectilePrefab;
    private float projectileSpeed = 5f;

    //[Header("Cone Settings")]
    private float coneAngle = 45f;         // Total spread angle
    private int bulletsPerShot = 5;

    //[Header("Firing Settings")]
    private int repeatCount = 3;
    private float delayBetweenShots = 0.5f;
    private float shootDirAngle = 0f;
    private bool targetPlayer = false;
    private float fireDelay;

    //handle whether to move or attack
    public override void FixedUpdate()
    {
        if (playerLoc != null && !isAttacking && !isStunned && !isDead)
        {
            Vector3 playerPos = playerLoc.position;
            if (Vector2.Distance(playerPos, transform.position) <= attackDistance && !isAttacking)
            {
                rb.velocity = Vector3.zero;
                //TODO Attack
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

    private void SetPattern(EnemyBulletPattern pattern)
    {
        currentPattern = pattern;

        projectilePrefab = pattern.projectilePrefab;
        projectileSpeed = pattern.projectileSpeed;

        coneAngle = pattern.coneAngle;
        bulletsPerShot = pattern.bulletsPerShot;

        repeatCount = pattern.repeatCount;
        delayBetweenShots = pattern.delayBetweenShots;

        shootDirAngle = pattern.shootDirAngle;
        targetPlayer = pattern.targetPlayer;
        fireDelay = pattern.fireDelay;
    }

    public void FireCone()
    {
        StartCoroutine(FireConeRoutine());
    }

    private IEnumerator FireConeRoutine()
    {
        yield return new WaitForSeconds(fireDelay);
        for (int i = 0; i < repeatCount; i++)
        {
            ShootCone();
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    private void ShootCone()
    {
        float startAngle;
        if(targetPlayer)
        {
            Vector2 direction = playerLoc.position - firePoint.position;
            float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            startAngle = angleToPlayer - (coneAngle / 2f);
        }else
        {
            startAngle = shootDirAngle - (coneAngle / 2f);
        }
        
        float angleStep = coneAngle / (bulletsPerShot - 1);

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = dir.normalized * projectileSpeed;
        }
    }
    /// <summary>
    /// swipe up attack
    /// </summary>
    public void AttackOneRight()
    {
        Debug.Log(currentAttackCooldown);
        if (currentAttackCooldown > 0) return;
        //attack and set attack cd
        currentAttackCooldown = Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        SetPattern(bulletPatterns[0]);
        patternIndex = 0;
        FireCone();
        Invoke(nameof(StopAttacking), attackAnimDuration1);

        if (!isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180, 0);
        }
    }

    public void AttackOneLeft()
    {
        if (currentAttackCooldown > 0) return;
        //attack and set attack cd
        currentAttackCooldown = Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        SetPattern(bulletPatterns[1]);
        patternIndex = 1;
        FireCone();
        Invoke(nameof(StopAttacking), attackAnimDuration1);

        if (isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0, -180, 0);
        }
    }

    public void AttackTwoRight()
    {
        if (currentAttackCooldown > 0) return;
        //attack and set attack cd
        currentAttackCooldown = Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        SetPattern(bulletPatterns[2]);
        patternIndex = 2;
        FireCone();
        Invoke(nameof(StopAttacking), attackAnimDuration2);

        if (!isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180, 0);
        }
    }

    public void AttackTwoLeft()
    {
        if (currentAttackCooldown > 0) return;
        //attack and set attack cd
        currentAttackCooldown = Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        SetPattern(bulletPatterns[3]);
        patternIndex = 3;
        FireCone();
        Invoke(nameof(StopAttacking), attackAnimDuration2);

        if (isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0, -180, 0);
        }
    }

    [Header("Jump Attack")]
    [SerializeField] private float pauseBeforeAttackTime = 0.25f;
    [SerializeField] private float jumpToAirTime = 1.0f;
    [SerializeField] private float distanceAbovePlayer = 2.0f;
    [SerializeField] private float waitAbovePlayerTime = .25f;
    [SerializeField] private float slamTime = .25f;
    [SerializeField] private float waitAfterSlamTime = .25f;
    public IEnumerator JumpAttack()
    {
        if (currentAttackCooldown > 0) yield break;
        //attack and set attack cd
        currentAttackCooldown = Stats.attackCooldown;
        rb.velocity = Vector3.zero;
        isAttacking = true;
        SetPattern(bulletPatterns[4]);
        patternIndex = 4;
        yield return new WaitForSeconds(pauseBeforeAttackTime);

        Vector2 playerLocOnAttack = playerLoc.position;

        //scale and jump to above the player
        transform.DOScale(1.3f, jumpToAirTime).SetEase(Ease.InSine);
        Vector2 posToJump = new Vector2(playerLocOnAttack.x, playerLocOnAttack.y + distanceAbovePlayer);
        transform.DOMove(posToJump, jumpToAirTime).SetEase(Ease.InSine);
        animator.CrossFade(Jump, 0, 0);
        currentState = Jump;
        LockState(Jump, jumpToAirTime);
        yield return new WaitForSeconds(jumpToAirTime + waitAbovePlayerTime);

        //slam down to player
        transform.DOScale(1.0f, slamTime).SetEase(Ease.InSine);
        transform.DOMove(playerLocOnAttack, slamTime).SetEase(Ease.InSine);
        animator.CrossFade(Fall, 0, 0);
        currentState = Fall;
        LockState(Fall, slamTime);

        yield return new WaitForSeconds(slamTime);

        animator.CrossFade(Idle, 0, 0);
        currentState = Idle;
        LockState(Idle, waitAfterSlamTime);

        FireCone();

        Invoke(nameof(StopAttacking), waitAfterSlamTime);
    }

    public void StartJumpAttack()
    {
        StartCoroutine(JumpAttack());
    }

    /// <summary>
    /// get animations state
    /// </summary>
    /// <returns></returns>
    public override int GetState()
    {
        if (Time.time < lockedTill) return currentState;

        //priorities
        if (isStunned) return LockState(TakeHit, stunAnimDuration);
        if (isDead) return LockState(Death, deadAnimDuration);
        if (isAttacking)
        {
            switch (patternIndex)
            {
                case 0:
                case 1:
                    return LockState(Attack1, attackAnimDuration1);
                case 2:
                case 3:
                    return LockState(Attack2, attackAnimDuration2);
                case 4:
                    break;
                default:
                    break;
            }
        }
        if (rb.velocity.magnitude > 0) return Run;
        else return Idle;
    }

    private int LockState(int s, float t)
    {
        lockedTill = Time.time + t;
        return s;
    }
}
