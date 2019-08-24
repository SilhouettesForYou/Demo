using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class UnderWater : MonoBehaviour
    {
        private Rigidbody2D playerRigibody;
        private float mass;
        // Start is called before the first frame update
        void Start()
        {
            mass = 95;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                playerRigibody = collider.gameObject.GetComponent<Rigidbody2D>();
                playerRigibody.mass = mass;
                EventCenter.Braodcast(EventType.Dive);
                EventCenter.Braodcast(EventType.WaterLeak);
                EventCenter.Braodcast<Transform>(EventType.DivingInPool, transform);
            }
        }
    }
}

