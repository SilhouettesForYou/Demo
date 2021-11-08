using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CupMonster : MonoBehaviour
    {
        private bool flip = false;
        public PankapuConfig config;
        private Transform leftClaw;
        private Transform rightClaw;
        private Transform vulnerability;

        private Animator animator;

        private bool isDead = false;
        private bool hitOnce = false;
        private bool isReset = false;
        private bool isFacingRight = false;

        private int sufferedCount = 0;
        private int maxSufferedCount = 2;
        public float movingSpeed = 1;
        private float patrolRadius = 1;

        private Vector2 startPoint;

        // Start is called before the first frame update
        void Start()
        {
            EventCenter.AddListener(EventType.AnRespawn, Reset);
            
            vulnerability = transform.Find("Vulnerability");

            startPoint = transform.position;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
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
           
            animator.SetBool("HitOnce", hitOnce);
            animator.SetBool("Dead", isDead);
            animator.SetBool("Reset", isReset);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!isDead)
            {
                Debug.Log(collider.transform.name);
                if (collider.transform.name == "ShockWave(Clone)")
                {
                    //Debug.Log(collider.transform.name);
                    sufferedCount++;
                    //Destroy(collider.gameObject);
                    if (sufferedCount == 1)
                    {
                        hitOnce = true;
                    }
                    if (sufferedCount == maxSufferedCount)
                    {
                        hitOnce = false;
                        isDead = true;
                        StartCoroutine(Delay());
                    }
                }
                if (collider.transform.name == "An'")
                {
                    EventCenter.Braodcast(EventType.PankapuDeath);
                    //Debug.Log("An' has been slain by " + transform.name + ", game over...");
                }
                if (collider.transform.name == "An")
                {
                    EventCenter.Braodcast(EventType.AnDeath);
                }
            }
            
        }

        void OnDsetroy()
        {

            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
        }
        private void Flip()
        {
            if (flip)
            {
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            }
            isFacingRight = !isFacingRight;
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
            isReset = true;
        }

        private void Reset()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
                hitOnce = false;
                isDead = false;
                isReset = false;
            }
            
        }
    }
}

