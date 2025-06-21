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
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float dashTime = .5f;
    [SerializeField] private float dashStrength = 3.0f;
    [SerializeField] private float dashCooldown = 1f;
    private float currentDashCooldown;
    private bool canMove = true;

    [Header("Attack")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackCooldown = 0.25f;
    [SerializeField] private float bulletSpeed = 5f;
    private float currentAttackCooldown;

    #region Unity Monobehavior
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(playerMoveDir.x * playerSpeed, playerMoveDir.y * playerSpeed);
        }

        if(fire.IsPressed())
        {
            Shoot();
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
}
