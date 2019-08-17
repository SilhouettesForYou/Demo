using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Forks : MonoBehaviour
    {
        private Dictionary<Transform, bool> forks;

        private float attackRange = 0.1f;
        private float downVelocity = -2.0f;
        private float time = 0;

        // Start is called before the first frame update
        void Start()
        {
            int num = transform.childCount;
            forks = new Dictionary<Transform, bool>();
            for (int i = 0; i < num; i++)
            {
                Transform fork = transform.GetChild(i);
                if (fork.name != "CupMonster")
                {
                    Rigidbody2D forkBody = fork.GetComponent<Rigidbody2D>();
                    if (i % 2 == 0)
                    {
                        forkBody.velocity = new Vector2(forkBody.velocity.x, downVelocity);
                        StartCoroutine(Delay(forkBody));
                        //fork.position = new Vector3(fork.position.x, fork.position.y - downDistance, fork.position.z);
                        forks.Add(fork, true);
                    }
                    else
                    {
                        forks.Add(fork, false);
                    }
                }
                
            }
        }
        // Update is called once per frame
        void Update()
        {
            List<Transform> fs = new List<Transform>(forks.Keys);
            for (int i = 0; i < fs.Count; i++)
            {
                Transform fork = fs[i];
                CheckForkOnPlayer(fork);
            }
        }
        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            List<Transform> fs = new List<Transform>(forks.Keys);
            if (Mathf.Abs(time - 2) < 0.0001)
            {
                for (int i = 0; i < fs.Count; i++)
                {
                    Transform fork = fs[i];
                    Rigidbody2D forkBody = fork.GetComponent<Rigidbody2D>();
                    //Debug.Log(fork.name + " " + forks[fork]);
                    if (forks[fork] == true)
                    {
                        forkBody.velocity = new Vector2(forkBody.velocity.x, -downVelocity);
                        forks[fork] = false;
                        StartCoroutine(Delay(forkBody));
                    }
                    else if (forks[fork] == false)
                    {
                        forkBody.velocity = new Vector2(forkBody.velocity.x, downVelocity);
                        forks[fork] = true;
                        StartCoroutine(Delay(forkBody));
                    }
                }
                time = 0;
            }
            
        }

        IEnumerator Delay(Rigidbody2D body)
        {
            yield return new WaitForSeconds(1.0f);
            body.velocity = Vector2.zero;
        }

        private void CheckForkOnPlayer(Transform fork)
        {
            Transform weapon = fork.Find("Weapon");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(weapon.position, attackRange, 1 << LayerMask.NameToLayer("Player"));
            foreach (var collider in colliders)
            {
                if (collider.transform.name == "An'")
                {
                    //Debug.Log("An' has been slain by " + fork.name + ", game over...");
                    EventCenter.Braodcast(EventType.IsAnPrimeDead);
                }
                if (collider.transform.name == "An")
                {
                    Debug.Log("An has been slain" + transform.name + ", game over...");
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.name == "An'")
            {
                Debug.Log("An has been slain" + transform.name + ", game over...");
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "ShockWave(Clone)")
            {
                Destroy(collider.gameObject);
            }
        }
    }
}


