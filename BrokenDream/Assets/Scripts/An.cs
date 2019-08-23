using UnityEngine;

namespace Demo
{
    public class An : MonoBehaviour
    {
        public float speed = 10.0f;

        private Rigidbody2D anRigidbody;
        private Transform pankapu;

        void Awake()
        {
            anRigidbody = GetComponent<Rigidbody2D>();
            if (FindObjectOfType<Pankapu>() != null)
            {
                pankapu = FindObjectOfType<Pankapu>().transform;
                Physics2D.IgnoreCollision(pankapu.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            anRigidbody.velocity = new Vector2(speed, anRigidbody.velocity.y);
        }
    }
}

