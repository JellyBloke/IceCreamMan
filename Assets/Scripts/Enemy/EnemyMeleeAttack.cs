﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float knockback = 3f;
    public float damage = 20f;

    Rigidbody2D enemyRb;

    private void Awake()
    {
        enemyRb = transform.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            PlayerHealthStatus health = collision.transform.GetComponent<PlayerHealthStatus>();
            health.TakeDamage(damage);
            Rigidbody2D playerRb = collision.transform.GetComponent<Rigidbody2D>();

            if(transform.position.x < playerRb.position.x)
            {
                // enemy contact from left
                playerRb.velocity = new Vector2(knockback, knockback);
            } else if(transform.position.x >= playerRb.position.x){
                // enemy contact from right
                playerRb.velocity = new Vector2(-knockback, knockback);
            }

            PlayerMovements m = collision.transform.GetComponent<PlayerMovements>();
            m.knockbackCount = m.knockbackLength;
        }
    }
}
