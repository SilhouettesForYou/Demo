using UnityEngine;

namespace Demo
{
    public class CameraTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Pankapu pankapu = collision.GetComponent<Pankapu>();
            if (pankapu != null)
            {
                EventCenter.Braodcast(EventType.BossCamera);
            }
        }
    }
}
