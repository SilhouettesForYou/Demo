using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Forks : MonoBehaviour
    {
        private float distance = 0f;
        private float pointRange = 0.1f;
        private float range = 4;

        private List<Transform> forks;
        private List<Vector3> starts;

        // Start is called before the first frame update
        void Start()
        {
            EventCenter.AddListener(EventType.CupBlowUp, CupBlowUp);
            EventCenter.AddListener(EventType.AnRespawn, Reset);

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
                CheckForkOnPlayer(forks[i]);
            }
        }

        public void CupBlowUp()
        {
            GameObject iron = Instantiate(Resources.Load("Prefabs/IronCube")) as GameObject;
            iron.transform.position = forks[0].position;
            forks[0].gameObject.SetActive(false);
        }

        private void CheckForkOnPlayer(Transform fork)
        {
            Transform weapon = fork.Find("Point");
            RaycastHit2D[] hits = Physics2D.RaycastAll(weapon.position, Vector2.down, pointRange,
                1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("AN"));
            foreach (var hit in hits)
            {
                if (hit.transform.name == "An'")
                {
                    EventCenter.Braodcast(EventType.PankapuDeath);
                }
                if (hit.transform.name == "An")
                {
                    EventCenter.Braodcast(EventType.AnDeath);
                }
            }
        }

        private void Reset()
        {
            forks[0].gameObject.SetActive(true);
            //Destroy(FindObjectOfType<Pushable>().gameObject);
        }
    }
}


