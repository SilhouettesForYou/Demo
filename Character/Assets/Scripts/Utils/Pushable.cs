using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Pushable : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        private Transform leftTrigger;
        private Transform rightTrigger;
        private Transform boxGroundCheck;
        private Rigidbody2D body;

        private float playerRdius = 0.5f;
        private float groundRadius = 0.5f;

        private bool dragableOrPushable = false;
        private bool isGrounded = true;

        void Awake()
        {
            body = transform.GetComponent<Rigidbody2D>();
            leftTrigger = transform.Find("LeftTrigger");
            rightTrigger = transform.Find("RightTrigger");
            boxGroundCheck = transform.Find("BoxGroundCheck");
        }

        void FixedUpdate()
        {
            CheckPushing(leftTrigger, playerRdius, playerInfo.player);
            CheckPushing(rightTrigger, playerRdius, playerInfo.player);
            if (dragableOrPushable && Input.GetKey(KeyCode.E))
            {
                // Release the constrain of X when the box is pushable or dragable, at the same tiem, 'E' pressed.
                body.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                //body.constraints &= RigidbodyConstraints2D.FreezeRotation;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                // Frezze the constrain of X when 'E' is up.
                body.constraints |= RigidbodyConstraints2D.FreezePositionX;
                dragableOrPushable = false;
            }

            CheckGround(boxGroundCheck, groundRadius, playerInfo.ground);

            if (!isGrounded)
            {
                // Release the constrain of Y when the box dropping.
                body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                body.constraints |= RigidbodyConstraints2D.FreezePositionX;
                //
            }
            else
            {
                //body.constraints &= RigidbodyConstraints2D.FreezePositionY;
            }
            if (!dragableOrPushable && body.velocity.y == 0)
            {
                body.constraints |= RigidbodyConstraints2D.FreezeAll;
            }
        }

        private void CheckGround(Transform trigger, float radius, LayerMask mask)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(trigger.position, radius, mask);
            //foreach (var item in colliders)
            //    Debug.Log(item);
            int id = Array.IndexOf(colliders, GameObject.Find("Ground").GetComponent<Collider2D>());
            if (id == -1)
            {
                isGrounded = false;
                dragableOrPushable = false;
                body.velocity = Vector2.down * 10;
            }
            else
            {
                isGrounded = true;
            }
        }

        private void CheckPushing(Transform trigger, float radius, LayerMask mask)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(trigger.position, radius, mask);
            foreach (var item in colliders)
            {
                if (item.transform.name == "An'")
                {
                    dragableOrPushable = true;
                }
            }
        }
    }
}
