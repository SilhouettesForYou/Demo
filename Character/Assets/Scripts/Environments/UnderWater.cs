using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class UnderWater : MonoBehaviour
    {
        public delegate void SetWaterLeak();
        public static event SetWaterLeak setWaterLeak;



        private Rigidbody2D playerRigibody;
        private float mass;
        // Start is called before the first frame update
        void Start()
        {
            mass = 40;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("touch the water");
            if (collider.transform.name == "An'")
            {
                playerRigibody = collider.gameObject.GetComponent<Rigidbody2D>();
                playerRigibody.mass = mass;
                EventCenter.Braodcast(EventType.Dive);
                setWaterLeak();
            }
        }
    }
}

