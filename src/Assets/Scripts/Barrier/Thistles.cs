using UnityEngine;

namespace Demo
{
    public class Thistles : MonoBehaviour
    {
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

        void OnTriggerEnter2D(Collider2D collider)
        {
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
}
