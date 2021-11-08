using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{ 
    public class TouchWater : MonoBehaviour
    {
        private An an;
        private Pankapu pankapu;
        private float anMass;
        private float pankapuMass;
        private Vector2 direction = Vector2.down * 1;
        private float band = 0.5f;
        private float offset = 1.5f;
        private Transform left;
        private Transform right;
        private float detectionLine = 15.0f;
        private bool isAnIn = false;
        private bool isPankapuIn = false;

        // Start is called before the first frame update
        void Start()
        {
            left = transform.Find("Left");
            right = transform.Find("Right");

            float width = PankapuConfig.ComputeRect(transform).x * transform.parent.localScale.x;
            left.position = new Vector3(transform.position.x - width / 2 + offset, 
                transform.position.y, transform.position.z);
            right.position = new Vector3(transform.position.x + width / 2 - offset, 
                transform.position.y, transform.position.z);
        }

        // Update is called once per frame
        void Update()
        {
            CheckIn();
            CheckOut();
            if (isAnIn)
            {
                CheckTouchWater(an.transform);
            }
            if (isPankapuIn)
            {
                CheckTouchWater(pankapu.transform);
            }
        }

        private void CheckIn()
        {
            if (!isAnIn)
            {
                RaycastHit2D hit = Physics2D.Raycast(left.position, Vector2.up, detectionLine, 
                    1 << LayerMask.NameToLayer("AN"));
                if (hit.transform != null)
                {
                    isAnIn = true;
                    an = hit.transform.GetComponent<An>();
                    anMass = an.GetComponent<Rigidbody2D>().mass;
                }
            }

            if (!isPankapuIn)
            {
                RaycastHit2D hit = Physics2D.Raycast(left.position, Vector2.up, detectionLine, 
                    1 << LayerMask.NameToLayer("Pankapu"));
                if (hit.transform != null)
                {
                    isPankapuIn = true;
                    pankapu = hit.transform.GetComponent<Pankapu>();
                    pankapuMass = pankapu.GetComponent<Rigidbody2D>().mass;
                }
            }

        }

        private void CheckOut()
        {
            if (isAnIn)
            {
                RaycastHit2D hit = Physics2D.Raycast(right.position, Vector2.up, detectionLine, 
                    1 << LayerMask.NameToLayer("AN"));
                if (hit.transform != null)
                {
                    isAnIn = false;
                    an.GetComponent<Rigidbody2D>().mass = anMass;
                }
            }
            if (isPankapuIn)
            {
                RaycastHit2D hit = Physics2D.Raycast(right.position, Vector2.up, detectionLine, 
                    1 << LayerMask.NameToLayer("Pankapu"));
                if (hit.transform != null)
                {
                    isPankapuIn = false;
                    pankapu.GetComponent<Rigidbody2D>().mass = pankapuMass;
                }
            }
        }

        private void CheckTouchWater(Transform player)
        {
            Transform groundCheck = player.Find("GroundCheck");

            Vector2 left = new Vector2(groundCheck.position.x - band, groundCheck.position.y);
            Vector2 right = new Vector2(groundCheck.position.x + band, groundCheck.position.y);

            RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(left, direction,
                1 << LayerMask.NameToLayer("Water"));
            RaycastHit2D[] hitsRight = Physics2D.RaycastAll(right, direction,
                1 << LayerMask.NameToLayer("Water"));

            Debug.DrawRay(left, direction, Color.cyan);
            Debug.DrawRay(right, direction, Color.cyan);

            string[] namesRayLeft = new string[hitsLeft.Length];
            for (int i = 0; i < hitsLeft.Length; i++)
            {
                namesRayLeft[i] = hitsLeft[i].collider.transform.name;
            }
            string[] namesRayRight = new string[hitsRight.Length];
            for (int i = 0; i < hitsRight.Length; i++)
            {
                namesRayRight[i] = hitsRight[i].collider.transform.name;
            }

            int iceCubeIdLeft = Array.IndexOf(namesRayLeft, "IceCube(Clone)");
            int bottomIdLeft = Array.IndexOf(namesRayLeft, "Bottom");
            int iceCubeIdRight = Array.IndexOf(namesRayRight, "IceCube(Clone)");
            int bottomIdRight = Array.IndexOf(namesRayRight, "Bottom");

            if (iceCubeIdLeft == -1 && iceCubeIdRight == -1)
            {
                if (bottomIdLeft != -1 && bottomIdRight != -1)
                {
                    player.GetComponent<Rigidbody2D>().mass = 40;
                }
            }
                
        }

    }
}
