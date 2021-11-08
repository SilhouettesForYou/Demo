using System;
using UnityEngine;

namespace Demo
{
    class WaterFalls : MonoBehaviour
    {
        private float width;
        private float height;

        private Transform leftTop;
        private Transform rightTop;


        private Animator animator;
        private bool stop = false;
        void Awake()
        {
            animator = transform.parent.GetComponent<Animator>();
            Physics2D.IgnoreCollision(transform.GetComponent<EdgeCollider2D>(),
                transform.parent.Find("Fish").GetComponent<CapsuleCollider2D>());

            Physics2D.IgnoreCollision(transform.GetComponent<EdgeCollider2D>(),
                FindObjectOfType<Pankapu>().GetComponent<CapsuleCollider2D>());

            Physics2D.IgnoreCollision(transform.GetComponent<EdgeCollider2D>(),
                FindObjectOfType<An>().GetComponent<CapsuleCollider2D>());

            width = PankapuConfig.ComputeRect(transform).x;
            height = PankapuConfig.ComputeRect(transform).y;
            width *= transform.parent.localScale.x;
            height *= transform.parent.localScale.y;

            leftTop = transform.Find("AncherLeftTop");
            rightTop = transform.Find("AncherRightTop");

            leftTop.position = new Vector3(transform.position.x - width / 2, transform.position.y + height / 2, transform.position.z);
            rightTop.position = new Vector3(transform.position.x + width / 2, transform.position.y + height / 2, transform.position.z);

            EventCenter.AddListener(EventType.AnRespawn, Reset);
        }


        void Update()
        {
            animator.SetBool("Stop", stop);
            leftTop.position = new Vector3(transform.position.x - width / 2, transform.position.y + height / 2, transform.position.z);
            rightTop.position = new Vector3(transform.position.x + width / 2, transform.position.y + height / 2, transform.position.z);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.GetComponent<IceBlock>() != null)
            {
                stop = true;
            }
            else
            {
                stop = false;
            }
        }

        private void Reset()
        {
            stop = false;
        }
    }
}
