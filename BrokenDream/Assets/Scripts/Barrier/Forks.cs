using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Forks : MonoBehaviour
    {
        private float attackRange = 0.1f;
        private float distance = 0f;
        private float range = 4;

        private List<Transform> forks;
        private List<Vector3> starts;

        // Start is called before the first frame update
        void Start()
        {
            EventCenter.AddListener<Transform>(EventType.CupBlowUp, CupBlowUp);
            forks = new List<Transform>();
            starts = new List<Vector3>();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "CupMonster")
                {
                    forks.Add(transform.GetChild(i));
                    starts.Add(transform.GetChild(i).position);
                }
            }
        }

        void Update()
        {
            distance += 0.05f;
            for (int i = 0; i < forks.Count; i++)
            {
                if (i % 2 == 0)
                {
                    forks[i].position = starts[i] + Vector3.down * Mathf.PingPong(distance, range);
                }
                else
                {
                    forks[i].position = starts[i] - Vector3.down * Mathf.PingPong(distance, range);
                }
            }
        }

        public void CupBlowUp(Transform fork)
        {
            StartCoroutine(ConverToIron(fork));
        }


        private IEnumerator ConverToIron(Transform fork)
        {
            yield return new WaitForSeconds(1.0f);
            GameObject iron = Instantiate(Resources.Load("Prefabs/IronCube")) as GameObject;
            iron.transform.position = fork.position;
            fork.gameObject.SetActive(false);
        }

        private void CheckForkOnPlayer(Transform fork)
        {
            Transform weapon = fork.Find("Point");
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


