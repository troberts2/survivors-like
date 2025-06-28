using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerControls playerControls;
    private InputAction move;
    private InputAction fire;
    private InputAction dash;

    private Vector2 playerMoveDir;

    private Rigidbody2D rb;
    [Header("Move and Dash")]
    [SerializeField] private float _playerSpeed = 3f;
    [SerializeField] private float dashTime = .5f;
    [SerializeField] private float dashStrength = 3.0f;
    [SerializeField] private float dashCooldown = 1f;
    private float currentDashCooldown;
    private bool canMove = true;

    [Header("Attack")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float _baseDamage = 2f;
    private float currentAttackCooldown;

    [Header("Animation")]
    private Animator animator;
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int TakeHit = Animator.StringToHash("take hit");
    private static readonly int Attack = Animator.StringToHash("attack");
    private bool isFacingRight = true;

    private float lockedTill;
    private int currentState;

    private bool isAttacking = false;
    [SerializeField] private float attackAnimDuration;

    private bool isStunned = false;
    [SerializeField] private float stunAnimDuration;

    public float BaseDamage { get => _baseDamage; set => _baseDamage = value; }
    public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }

    #region Unity Monobehavior
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        animator.CrossFade(Idle, 0, 0);
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();

        dash = playerControls.Player.Dash;
        dash.Enable();
        dash.performed += Dash;
    }

    private void OnDisable()
    {
        move.Disable();

        fire.Disable();

        dash.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveDir = move.ReadValue<Vector2>();

        if (currentDashCooldown >= 0) currentDashCooldown -= Time.deltaTime;

        if (currentAttackCooldown >= 0) currentAttackCooldown -= Time.deltaTime;

        var state = GetState();

        if (state != currentState)
        {
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }

        if (playerMoveDir.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180, 0);
        }
        else if (playerMoveDir.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0, -180, 0);
        }

        if (fire.IsPressed())
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (canMove && !isAttacking)
        {
            rb.velocity = new Vector2(playerMoveDir.x * PlayerSpeed, playerMoveDir.y * PlayerSpeed);
        }
        
    }
    #endregion

    #region Input Action Callbacks

    private void Shoot()
    {
        if(currentAttackCooldown > 0)
        {
            return;
        }

        currentAttackCooldown = attackCooldown;
        isAttacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke(nameof(StopAttacking), attackAnimDuration);
        GameObject newBullet = Instantiate(attackPrefab, transform.position, Quaternion.identity);

        //get mouse pos and direction to shoot
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position);
        direction = direction.normalized;
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    #endregion

    #region Move Functions

    private void Dash(InputAction.CallbackContext context)
    {
        if (currentDashCooldown > 0) return;

        canMove = false;
        rb.velocity = Vector2.zero;
        currentDashCooldown = dashCooldown;
        rb.AddForce(playerMoveDir * dashStrength, ForceMode2D.Impulse);
        Invoke(nameof(EnableMovement), dashTime);
    }

    private void EnableMovement()
    {
        rb.velocity = Vector2.zero;
        canMove = true;
    }

    #endregion

    private int GetState()
    {
        if (Time.time < lockedTill) return currentState;

        //priorities
 
        if (isAttacking) return LockState(Attack, attackAnimDuration);
        if (isStunned) return LockState(TakeHit, stunAnimDuration);
        var state = playerMoveDir != Vector2.zero ? Run : Idle;
        return state;

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
}
