using UnityEngine;
using System.Collections;

namespace Demo
{
    public class An : MonoBehaviour
    {
        private float originalSpeed;
        public float speed = 15.0f;
        public PankapuConfig config;

        private Rigidbody2D anRigidbody;
        private Transform pankapu;
        private Transform headCheck;
        private Transform groundCheck;
        private Animator animator;

        private CheckPoint lastCheckPoint = null;
        private Vector3 startPosition;
        private float lastPositionX;
        private float lastX;
        private float interval = 0.5f;
        private float time = 0;
        private float timer = 0;
        private float mass;

        public bool isDead = false;
        public bool isDeading = false;
        public bool isBlocking = false;

        void Awake()
        {
            headCheck = transform.Find("HeadCheck");
            groundCheck = transform.Find("GroundCheck");
            originalSpeed = speed;
            startPosition = transform.position;
            lastX = startPosition.x;
            anRigidbody = GetComponent<Rigidbody2D>();
            animator = transform.GetComponent<Animator>();
            mass = anRigidbody.mass;
            if (FindObjectOfType<Pankapu>() != null)
            {
                pankapu = FindObjectOfType<Pankapu>().transform;
                Physics2D.IgnoreCollision(pankapu.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
            }
            EventCenter.AddListener(EventType.AnDeath, CheckDeath);
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckBlockDead());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isDead)
            {
                SpeedUpOnOil speedUpOnOil = GetComponent<SpeedUpOnOil>();
                float speedOfX = speed;
                if (speedUpOnOil != null && speedUpOnOil.isOnOil())
                {
                    Debug.Log("speed up");
                    speedOfX *= speedUpOnOil.speedupRate;
                }
                anRigidbody.velocity = new Vector2(speedOfX, anRigidbody.velocity.y);
            }
            else
            {
                anRigidbody.velocity = new Vector2(0, anRigidbody.velocity.y);
            }
            lastPositionX = transform.position.x;

            CheckDroppedIceCube();

            if (isDead)
            {
                EventCenter.Braodcast(EventType.AnDeath);
                timer += Time.deltaTime;
                if (timer > 2.10f)
                {
                    isDead = false;
                    EventCenter.Braodcast(EventType.AnRespawn);
                    Respawn();
                    timer = 0;
                }
            }

            CheckMove();
        }

        void Update()
        {
            animator.SetBool("Block", isBlocking);
            animator.SetBool("Dead", isDead);
        }

        void OnDestroy()
        {
            //EventCenter.RemoveListener(EventType.AnDeath, CheckDeath);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "PondBottom")
            {
                EventCenter.Braodcast(EventType.AnDeath);
            }
        }

        public void CheckMove()
        {
            time += Time.deltaTime;
            //Debug.Log(time);
            if (time > interval)
            {
                if (Mathf.Abs(lastX - transform.position.x) < 0.0001)
                {
                    isBlocking = true;
                    //AudioManager.PlayEffect("MusicScream-2");
                }
                else
                {
                    isBlocking = false;
                }
                time = 0;
                lastX = transform.position.x;
            }
        }

        private IEnumerator CheckBlockDead()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);
                if (Mathf.Abs(lastPositionX - transform.position.x) < 0.0001)
                {
                    if (isBlocking)
                        isDead = true;
                }
            }


        }

        private void CheckDroppedIceCube()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(headCheck.position, 0.3f, config.pushable);

            foreach(var collider in colliders)
            {
                if (collider.transform.name == "PushableIceCube-1")
                {
                    isDead = true;
                }
            }
        }

        private void CheckDeath()
        {
            isDead = true;
        }

        public void SetCheckPoint(CheckPoint checkpoint)
        {
            lastCheckPoint = checkpoint;
        }
        

        public void Respawn()
        {
            if (lastCheckPoint != null)
            {
                transform.position = lastCheckPoint.transform.position;
            }
            else
            {
                transform.position = startPosition;
            }
            anRigidbody.mass = mass;
            EventCenter.Braodcast<FocusOn>(EventType.FocusOn, FocusOn.FocusOnPankapu);
            speed = originalSpeed;
            isBlocking = false;
            isDeading = false;
        }
    }
}

