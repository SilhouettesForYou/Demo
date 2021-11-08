using UnityEngine;

namespace Demo
{
    public class TrashCover : MonoBehaviour
    {
        private Vector3 startPosition;
        private Quaternion startRotation;

        void Awake()
        {
            startPosition = transform.position;
            startRotation = transform.localRotation;
            EventCenter.AddListener(EventType.AnRespawn, Reset);
        }

        void OnDestroy()
        {

            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
        }

        private void Reset()
        {
            transform.position = startPosition;
            transform.localRotation = startRotation;
        }
    }
}
