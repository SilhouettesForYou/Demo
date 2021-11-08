using UnityEngine;

namespace Demo
{
    public class CameraZoomOut : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            An an = collision.GetComponent<An>();
            if (an != null)
            {
                EventCenter.Braodcast(EventType.ZoomOut);
                EventCenter.Braodcast(EventType.PankapuDeath);
            }
        }
    }
}
