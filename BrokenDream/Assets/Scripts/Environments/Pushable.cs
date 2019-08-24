﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Pushable : MonoBehaviour
    {
        private Transform boxGroundCheckLeft;
        private Transform boxGroundCheckRight;
        private Rigidbody2D body;

        private Rigidbody2D player;
        private FixedJoint2D joint;
        private bool isAttached = false;
        private bool isGrounded = true;
        private bool isFacingRight = true;

        private float relativeWidth;
        private float relativeHeight;

        void Awake()
        {
            body = transform.GetComponent<Rigidbody2D>();
            boxGroundCheckLeft = transform.Find("GroundCheckLeft");
            boxGroundCheckRight = transform.Find("GroundCheckRight");
            joint = GetComponent<FixedJoint2D>();
            joint.enabled = false;
            EventCenter.AddListener<bool, Rigidbody2D>(EventType.Attach, CheckPush);
            EventCenter.AddListener<bool>(EventType.Facing, CheckFacing);
        }

        void FixedUpdate()
        {
            if (isAttached)
            {
                ComputeConnectedAncher(isFacingRight);
                // Release the constrain of X when the box is pushable or dragable, at the same tiem, 'E' pressed.
                body.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                joint.enabled = true;
                joint.connectedBody = player;
                joint.connectedAnchor = new Vector2(relativeWidth, relativeHeight);
                joint.anchor = new Vector2(0f, 0f);
            }
            else
            {
                body.constraints |= RigidbodyConstraints2D.FreezePositionX;
                //joint.connectedAnchor = new Vector2(0, 0);
                joint.enabled = false;
                joint.connectedBody = null;
            }

            CheckGround(boxGroundCheckLeft, boxGroundCheckRight);

            if (!isGrounded && !isAttached)
            {
                // Release the constrain of Y when the box dropping.
                body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                body.constraints |= RigidbodyConstraints2D.FreezePositionX;
            }
        }

        private void ComputeConnectedAncher(bool facingRight)
        {
            // original rect
            Rect pankapuRect = FindObjectOfType<Pankapu>().transform.GetComponent<SpriteRenderer>().sprite.rect;
            Rect boxRect = transform.GetComponent<SpriteRenderer>().sprite.rect;

            float pixelsPankapu = FindObjectOfType<Pankapu>().transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            float pixelsBox = transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

            relativeWidth = (isFacingRight ? 1 : -1) * 
                (pankapuRect.width / (2 * pixelsPankapu) + boxRect.width / (2 * pixelsBox));
            relativeHeight = -boxRect.height / (2 * pixelsBox);
        }

        private void CheckGround(Transform left, Transform right)
        {
            Vector3 direction = Vector3.down;
            RaycastHit2D colliderLeft = Physics2D.Raycast(left.position, direction, 1 << LayerMask.NameToLayer("Ground"));
            RaycastHit2D colliderRight = Physics2D.Raycast(right.position, direction, 1 << LayerMask.NameToLayer("Ground"));

            Debug.DrawRay(left.position, direction, Color.cyan);
            Debug.DrawRay(right.position, direction, Color.cyan);

            if (colliderLeft.transform != null && colliderRight.transform != null)
            {
                isGrounded = false;
            }
        }

        public void CheckPush(bool flag, Rigidbody2D body)
        {
            isAttached = flag;
            player = body;
        }

        private void CheckFacing(bool flag)
        {
            isFacingRight = flag;
        }

    }
}
