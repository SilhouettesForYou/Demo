using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Pushable : MonoBehaviour
    {
        public PankapuConfig config;
        private Transform boxGroundCheckLeft;
        private Transform boxGroundCheckRight;
        private Rigidbody2D body;

        private Rigidbody2D player;
        private FixedJoint2D joint;
        private BoxCollider2D box;
        private float rayLength = 0.5f;
        private bool isAttached = false;
        private bool isGrounded = true;
        private bool isFacingRight = true;

        private float relativeWidth;
        private float relativeHeight;
        private float ancherHeight;
        private Vector3 startPosition;

        void Awake()
        {
            startPosition = transform.position;
            body = transform.GetComponent<Rigidbody2D>();
            box = transform.GetComponent<BoxCollider2D>();
            boxGroundCheckLeft = transform.Find("GroundCheckLeft");
            boxGroundCheckRight = transform.Find("GroundCheckRight");
            joint = GetComponent<FixedJoint2D>();
            joint.enabled = false;
            EventCenter.AddListener<bool, Rigidbody2D, string>(EventType.Attach, CheckPush);
            EventCenter.AddListener<bool>(EventType.Facing, CheckFacing);
            EventCenter.AddListener(EventType.AnRespawn, Reset);
        }

        void FixedUpdate()
        {
            //Debug.Log(transform.name + " " + isAttached);
            if (isAttached)
            {
                ComputeConnectedAncher(isFacingRight);
                // Release the constrain of X when the box is pushable or dragable, at the same tiem, 'E' pressed.
                body.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                joint.enabled = true;
                joint.connectedBody = player;
                joint.connectedAnchor = new Vector2(relativeWidth, relativeHeight);
                //Debug.Log(player.position);
                if (transform.name == "PushableIceCube")
                    joint.anchor = new Vector2(0, -0.25f);
                if (transform.name == "IronCube(Clone)")
                    joint.anchor = new Vector2(0, -1.5f);
            }
            else
            {
                body.constraints |= RigidbodyConstraints2D.FreezePositionX;
                joint.connectedAnchor = new Vector2(0, 0);
                joint.enabled = false;
                joint.connectedBody = null;
            }

            CheckGround(boxGroundCheckLeft, boxGroundCheckRight);
            body.constraints |= RigidbodyConstraints2D.FreezeRotation;
            if (!isGrounded && isAttached)
            {
                isAttached = false;
            }
            if (!isGrounded && !isAttached)
            {
                // Release the constrain of Y when the box dropping.
                InputManager.InteractiveBtnDown = false;
                body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                body.constraints |= RigidbodyConstraints2D.FreezePositionX;
                body.AddForce(new Vector2(0, -100));
            }
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
            EventCenter.RemoveListener<bool, Rigidbody2D, string>(EventType.Attach, CheckPush);
            EventCenter.RemoveListener<bool>(EventType.Facing, CheckFacing);
        }


        private void ComputeConnectedAncher(bool facingRight)
        {
            // original rect
            Rect pankapuRect = FindObjectOfType<Pankapu>().transform.GetComponent<SpriteRenderer>().sprite.rect;
            Rect boxRect = transform.GetComponent<SpriteRenderer>().sprite.rect;

            float pixelsPankapu = FindObjectOfType<Pankapu>().transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            float pixelsBox = transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

            relativeWidth = (isFacingRight ? 1 : -1) * (pankapuRect.width / (2 * pixelsPankapu) + boxRect.width / (2 * pixelsBox));
            relativeHeight = player.position.y;
            ancherHeight = -box.size.y / 2;
            //Debug.Log("width: " + relativeWidth);
            //Debug.Log("height: " + relativeHeight);
            //Debug.Log("ancher height: " + ancherHeight / 2);
            //relativeHeight = -box.size.y / 2;
            //Debug.Log(-box.size.y);
            //relativeWidth = player.position.x;
            //relativeHeight = transform.position.y;
        }

        private void CheckGround(Transform left, Transform right)
        {
            Vector3 direction = Vector3.down;
            RaycastHit2D colliderLeft = Physics2D.Raycast(left.position, direction, rayLength, config.ground | config.movingPlatform);
            RaycastHit2D colliderRight = Physics2D.Raycast(right.position, direction, rayLength, config.ground | config.movingPlatform);
            Debug.DrawRay(left.position, direction, Color.cyan);
            Debug.DrawRay(right.position, direction, Color.cyan);

            if (colliderLeft.transform == null && colliderRight.transform == null)
            {
                isGrounded = false;
            }
            else
            {
                isGrounded = true;
            }
        }

        public void CheckPush(bool flag, Rigidbody2D body,string name)
        {
            isAttached = flag;
            player = body;
            //Debug.Log(transform.name + " " + name);
            if (transform != null)
            {
                if (!name.Equals(transform.name))
                {
                    isAttached = false;
                }
            }
            
        }

        private void CheckFacing(bool flag)
        {
            isFacingRight = flag;
        }

        private void Reset()
        {
            isAttached = false;
            transform.position = startPosition;
            joint.connectedBody = null;
            joint.connectedAnchor = new Vector2(0, 0);
            joint.enabled = false;
            body.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
