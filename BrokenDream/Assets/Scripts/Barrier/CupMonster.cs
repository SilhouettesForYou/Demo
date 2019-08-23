﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CupMonster : MonoBehaviour
    {
        public PankapuConfig config;
        private Transform leftClaw;
        private Transform rightClaw;
        private Transform vulnerability;

        private Animator animator;

        private bool isDead = false;
        private bool isFacingRight = false;
        public bool isMoving = true;

        private int sufferedCount = 0;
        private int maxSufferedCount = 2;
        private float attackRange = 1.0f;
        private float forkRadius = 0.1f;
        public float movingSpeed = 1;
        private float patrolRadius = 1;

        private Vector2 startPoint;

        // Start is called before the first frame update
        void Start()
        {
            leftClaw = transform.Find("LeftClaw");
            rightClaw = transform.Find("RightClaw");
            vulnerability = transform.Find("Vulnerability");

            startPoint = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                if (isFacingRight)
                {
                    Vector2 vec = transform.position;
                    vec.x += Time.deltaTime * movingSpeed;

                    if (vec.x <= startPoint.x + patrolRadius)
                        transform.position = vec;
                    else
                        Flip();
                }
                else
                {
                    Vector2 vec = transform.position;
                    vec.x -= Time.deltaTime * movingSpeed;

                    if (vec.x >= startPoint.x - patrolRadius)
                        transform.position = vec;
                    else
                        Flip();
                }

                CheckAttackRange(leftClaw);
                CheckAttackRange(rightClaw);
            }
            else
            {
                if (!isDead)
                {
                    Collider2D[] colliders = 
                        Physics2D.OverlapCircleAll(vulnerability.position, forkRadius, config.monster);
                    foreach (var item in colliders)
                    {
                        if (item.transform.name == "Fork-1")
                        {
                            EventCenter.Braodcast<Transform>(EventType.CupBlowUp, item.transform);
                            isDead = true;
                            Destroy(gameObject, 1);
                        }
                    }
                }
            }

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (transform.name == "CupMonsterBoss")
                maxSufferedCount = 100;
            if (collider.transform.name == "ShockWave(Clone)")
            {
                Destroy(collider.gameObject);
                if (sufferedCount == maxSufferedCount)
                    Destroy(gameObject);
                sufferedCount++;
            }
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= 1;
            transform.localScale = scale;
            isFacingRight = !isFacingRight;
        }

        private void CheckAttackRange(Transform claw)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(claw.position, attackRange, 1 << LayerMask.NameToLayer("Player"));
            foreach (var collider in colliders)
            {
                if (collider.transform.name == "An'")
                {
                    EventCenter.Braodcast(EventType.IsAnPrimeDead);
                    //Debug.Log("An' has been slain by " + transform.name + ", game over...");
                }
                if (collider.transform.name == "An")
                {
                    Debug.Log("An has been slain" + transform.name + ", game over...");
                }
            }
        }

    }
}

