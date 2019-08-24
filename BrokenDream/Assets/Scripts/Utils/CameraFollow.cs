using UnityEngine;

namespace Demo
{
    public class CameraFollow : MonoBehaviour
    {
        public float cameraSpeed = 5f;
        public float maxX = 32f;
        public float minX = 0f;

        private bool focusOnPankapu = true;
        private bool focusOnAn = false;

        private Transform an;
        private Transform pankapu;
        private Transform pool;

        void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            if (FindObjectOfType<An>() != null)
                an = FindObjectOfType<An>().transform;
            if (FindObjectOfType<Pankapu>() != null)
                pankapu = FindObjectOfType<Pankapu>().transform;
            if (FindObjectOfType<UnderWater>() != null)
                pool = FindObjectOfType<UnderWater>().transform;

            if (an != null && pankapu == null)
            {
                minX = an.transform.position.x;
                focusOnAn = true;
                focusOnPankapu = false;
            }
            else
            {
                minX = pankapu.transform.position.x;
                focusOnAn = false;
                focusOnPankapu = true;
            }
            EventCenter.AddListener<bool>(EventType.FocusOn, ChangeFocusOn);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vec = transform.position;
            if (focusOnPankapu)
            {
                FocusOn(pankapu, vec);
            }
            else if (focusOnAn)
            {
                FocusOn(an, vec);
            }
            else
            {
                vec.x = pool.position.x;
                vec.y = pool.position.y;
                transform.position = vec;
            }
        }

        private void FocusOn(Transform t, Vector3 vec)
        {
            if (t.position.x <= maxX && t.position.x >= minX)
            {
                vec.x = t.position.x;
            }
            vec.y = Mathf.Lerp(vec.y, t.position.y, Time.deltaTime * cameraSpeed);
            transform.position = vec;
        }

        private void ChangeFocusOn(bool flag)
        {
            focusOnPankapu = flag;
        }
    }
}

