using UnityEngine;

namespace Demo
{
    public class LotusLeaf : MonoBehaviour
    {
        private An an;
        private HingeJoint2D hinge;
        private Transform left;
        private Transform right;

        private bool isAnOnLotus = false;
        private float speedOfAn;
        private float detectionLine = 5.0f;
        private float offset = 0.5f;
        void Awake()
        {
            hinge = GetComponent<HingeJoint2D>();
            left = transform.Find("Left");
            right = transform.Find("Right");

            float width = PankapuConfig.ComputeRect(transform).x * transform.parent.localScale.x;
            left.position = new Vector3(transform.position.x - width / 2 + offset, transform.position.y, transform.position.z);
            right.position = new Vector3(transform.position.x + width / 2  - offset, transform.position.y, transform.position.z);
        }

        void Update()
        {
            CheckIn();
            CheckOut();
            //angle = hinge.jointAngle;
            if (isAnOnLotus)
            {
                //if (angle < 0)
                //{
                //    //an.speed = speedOfAn / ((angle + offset) *  friction);
                //    an.speed = 0.1f;
                //}
                //else if (angle > 0)
                //{
                //    an.speed = speedOfAn * ((angle + offset) / friction);
                //}
                //else
                //{
                //    an.speed = speedOfAn;
                //}
            }
        }

        private void CheckIn()
        {
            if (!isAnOnLotus)
            {
                RaycastHit2D hit = Physics2D.Raycast(left.position, Vector2.up, detectionLine, 1 << LayerMask.NameToLayer("AN"));
                if (hit.transform != null)
                {
                    isAnOnLotus = true;
                    an = hit.transform.GetComponent<An>();
                    speedOfAn = an.speed;
                }
            }

            //if (!isPankapuOnLotus)
            //{
            //    RaycastHit2D hit = Physics2D.Raycast(left.position, Vector2.up, detectionLine, 1 << LayerMask.NameToLayer("Player"));
            //    if (hit.transform != null)
            //    {
            //        isPankapuOnLotus = true;
            //        speedOfPankapu = hit.transform.GetComponent<Pankapu>().config.walkSpeed;
            //        hit.transform.GetComponent<Pankapu>().config.walkSpeed = speedOnLotus;
            //    }
            //}
            
        }

        private void CheckOut()
        {
            if (isAnOnLotus)
            {
                RaycastHit2D hit = Physics2D.Raycast(right.position, Vector2.up, detectionLine, 1 << LayerMask.NameToLayer("AN"));
                if (hit.transform != null)
                {
                    isAnOnLotus = false;
                    an.speed = speedOfAn;
                }
            }
            //if (isPankapuOnLotus)
            //{
            //    RaycastHit2D hit = Physics2D.Raycast(left.position, Vector2.up, detectionLine, 1 << LayerMask.NameToLayer("Player"));
            //    if (hit.transform != null)
            //    {
            //        isPankapuOnLotus = false;
            //        hit.transform.GetComponent<Pankapu>().config.walkSpeed = speedOfPankapu;
            //    }
            //}
        }
    }
}
