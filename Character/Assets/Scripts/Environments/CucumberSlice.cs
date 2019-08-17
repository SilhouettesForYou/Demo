using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CucumberSlice : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, 10);
        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.name == "An'")
            {
                Debug.Log("An' has been slain by " + transform.name + ", game over...");
            }

            if (collision.transform.name == "An")
            {
                Debug.Log("An has been slain by " + transform.name + ", game over...");
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

