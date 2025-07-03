using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerHealth ph;
    private PlayerUpgrade pu;

    private void Start()
    {
        ph = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        pu = GetComponent<PlayerUpgrade>();
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
                ph.ChangeHealth(-enemy.Stats.damage);
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
                ph.ChangeHealth(-enemy.Stats.damage);
            }
        }

        if(collider.CompareTag("Experience"))
        {
            XPBehavior xp = collider.GetComponent<XPBehavior>();
            if (xp != null)
            {
                xp.GoToPlayer(transform, xp.PlayerCollectDelay, pu);
            }
        }
    }
}
