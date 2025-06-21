using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private float enemyDamage = 1f;

    [SerializeField] private float attackCooldown = 1.0f;
    private float currentAttackCooldown;

    private Rigidbody2D rb;

    public Transform playerLocation;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAttackCooldown >= 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(playerLocation != null)
        {
            Vector2 moveDir = (playerLocation.position - transform.position).normalized;
            rb.velocity = moveDir * enemySpeed;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var collider = collision.collider;

        if(collider.CompareTag("Player"))
        {
            AttackPlayer(collider.GetComponent<PlayerHealth>());
        }
    }

    private void AttackPlayer(PlayerHealth ph)
    {
        //dont attack if on cd
        if(currentAttackCooldown > 0) { return; }

        //attack and set attack cd
        currentAttackCooldown = attackCooldown;
        ph.ChangeHealth(-enemyDamage);
    }
}
