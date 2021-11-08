using UnityEngine;

namespace Demo
{
    public class LightningDamage : MonoBehaviour
    {
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
