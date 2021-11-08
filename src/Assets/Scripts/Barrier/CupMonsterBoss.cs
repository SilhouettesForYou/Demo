using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CupMonsterBoss : MonoBehaviour
    {
        public PankapuConfig config;
        private Transform vulnerability;
        private Transform lightning;
        private Animator animator;
        private Vector2 startPoint;

        private bool isDead = false;
        private bool isOpen = false;
        private bool isFacingRight = false;

        private float radius = 0.1f;
        private float timer = 0f;
        private float lightningDelay = 1.0f;
        private float interval = 2.0f;
        public float movingSpeed = 2;
        private float patrolRadius = 3;
        // Start is called before the first frame update
        void Awake()
        {
            EventCenter.AddListener(EventType.AnRespawn, Reset);
            EventCenter.AddListener(EventType.BossCamera, Open);
            
            vulnerability = transform.Find("Vulnerability");
            lightning = transform.Find("Lightning");

            startPoint = transform.position;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetBool("Open", isOpen);
            animator.SetBool("Dead", isDead);

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

            timer += Time.deltaTime;
            if (timer > interval)
            {
                isDead = false;
                lightning.gameObject.SetActive(true);
            }
            if (timer > interval + lightningDelay)
            {
                timer = 0;
                interval = Random.Range(1.0f, 4.0f);
                lightning.gameObject.SetActive(false);
            }

            Collider2D collider = Physics2D.OverlapCircle(vulnerability.position, radius, 1 << LayerMask.NameToLayer("Pankapu"));
            if (collider != null)
            {
                if (collider.transform.GetComponent<Pankapu>() != null && InputManager.SkillBtnDown)
                {
                    isDead = true;
                    EventCenter.Braodcast(EventType.PankapuDeath);
                    transform.gameObject.SetActive(false);
                }
            }
            
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
            EventCenter.RemoveListener(EventType.BossCamera, Open);
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.GetComponent<Pankapu>() != null)
            {
                EventCenter.Braodcast(EventType.PankapuDeath);
                EventCenter.Braodcast<FocusOn>(EventType.FocusOn, FocusOn.FocusOnAn);
            }
            if (collision.transform.GetComponent<An>() != null)
            {
                EventCenter.Braodcast(EventType.AnDeath);
            }
        }

        private void Open()
        {
            isOpen = true;
        }

        private void Reset()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
                isDead = false;
                isOpen = false;
            }
            
        }
    }
}

