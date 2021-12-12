﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    public float detectionRange;
    public float patrolRange;

    public float knockbackCount;
    public float knockbackLength;

    Rigidbody2D entity;
    bool isLeft;

    // initial position
    Vector2 initialPos;
    Vector2 finalPosLeft;
    Vector2 finalPosRight;


    private void Awake()
    {
        entity = GetComponent<Rigidbody2D>();
        initialPos = entity.position;
        SetFinalPatrolPosition();
        isLeft = true;
    }

    void SetFinalPatrolPosition()
    {
        finalPosLeft.x = initialPos.x - patrolRange;
        finalPosRight.x = initialPos.x + patrolRange;

        finalPosLeft.y = finalPosRight.y = initialPos.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(knockbackCount <= 0)
        {
            if (Mathf.Abs(target.position.x - entity.position.x) <= detectionRange)
            {
                ChasePlayer();
            }
            else
            {
                // patrolling
                Patrol();
            }
        }
        else
        {
            knockbackCount -= Time.deltaTime;
        }
    }

    void Patrol()
    {
        if(transform.position.x < finalPosLeft.x)
        {
            isLeft = false;
            entity.velocity = new Vector2(moveSpeed, entity.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if(transform.position.x > finalPosRight.x)
        {
            isLeft = true;
            entity.velocity = new Vector2(-moveSpeed, entity.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x > finalPosLeft.x && isLeft)
        {
            entity.velocity = new Vector2(-moveSpeed, entity.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (!isLeft && transform.position.x < finalPosRight.x)
        { 
            entity.velocity = new Vector2(moveSpeed, entity.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (Mathf.Abs(transform.position.x - finalPosLeft.x) <= 0.01f)
        {
            isLeft = false;
        }
           
        else if (Mathf.Abs(transform.position.x - finalPosRight.x) <= 0.01f)
        {
            isLeft = true;
        }
            
    }

    void ChasePlayer()
    {
        if(target.position.x < entity.position.x)
        {
            entity.velocity = new Vector2(-moveSpeed, entity.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } 
        else if(target.position.x > entity.position.x)
        {
            entity.velocity = new Vector2(moveSpeed, entity.velocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // still bug if collide with wall
        if (collision.transform.gameObject.CompareTag("WorldWall"))
        {
            if (!isLeft)
            {
                isLeft = true;
                finalPosRight.x = transform.position.x;
            } else if (isLeft)
            {
                isLeft = false;
                finalPosLeft.x = transform.position.x;
            }
        }
    }
}
