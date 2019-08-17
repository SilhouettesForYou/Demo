using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{ 
    public class TouchWater : MonoBehaviour
    {
        public delegate void ChangeStateToSwim();
        public static event ChangeStateToSwim changeStateToSwim;

        public PlayerInfo playerInfo;
        private Transform player;
        private Rigidbody2D playerRigibody;
        private float mass;
        private Vector2 direction = Vector2.down * 1;
        private float band = 0.1f;
        private Vector2 boxSize = new Vector2(0.1f, 1.0f);
        // Start is called before the first frame update
        void Start()
        {
            mass = 40;
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {

                Transform groundCheck =player.Find("GroundCheck");

                Vector2 left = new Vector2(groundCheck.position.x - band, groundCheck.position.y);
                Vector2 right = new Vector2(groundCheck.position.x + band, groundCheck.position.y);

                RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(left, direction, playerInfo.water);
                RaycastHit2D[] hitsRight = Physics2D.RaycastAll(right, direction, playerInfo.water);

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
                        //Debug.Log("sink");
                        playerRigibody = player.GetComponent<Rigidbody2D>();
                        playerRigibody.mass = mass;
                        changeStateToSwim();
                    }
                }

            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                player = collider.transform;
            }
        }

    }
}
