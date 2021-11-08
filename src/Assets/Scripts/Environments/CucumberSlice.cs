using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CucumberSlice : MonoBehaviour
    {
        Transform kitchKnife;
        // Start is called before the first frame update
        void Start()
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), 
                FindObjectOfType<KnifeMovement>().GetComponent<Collider2D>());

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
                EventCenter.Braodcast(EventType.PankapuDeath);
                //Debug.Log("An' has been slain by " + transform.name + ", game over...");
            }

            if (collision.transform.name == "An")
            {
                EventCenter.Braodcast(EventType.AnDeath);
            }
        }
    }
}

