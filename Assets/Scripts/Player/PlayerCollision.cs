using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerHealth ph;

    private void Start()
    {
        ph = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        var collider = collision.collider;
        if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyAttack"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy == null) collider.GetComponentInParent<Enemy>();

            if(enemy != null)
            {
                ph.ChangeHealth(-enemy.stats.damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyAttack"))
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy == null) enemy = collider.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                ph.ChangeHealth(-enemy.stats.damage);
            }
        }
    }
}
