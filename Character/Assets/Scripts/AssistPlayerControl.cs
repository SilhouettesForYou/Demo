﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class AssistPlayerControl : MonoBehaviour
    {
        private StateMachine<AssistPlayerControl> m_StateMachine;
        public StateMachine<AssistPlayerControl> StateMachine
        { 
            get
            {
                if (m_StateMachine == null)
                    m_StateMachine = new StateMachine<AssistPlayerControl>(this, PlayerIdleState.Instance, PlayerGlobalState.Instance);
                return m_StateMachine;
            }
        }

        public PlayerInfo playerInfo;

        public GameObject iceCube;
        public GameObject iceCubeCell;

        //[HideInInspector]
        //public Control control;
        [HideInInspector]
        public Rigidbody2D rigid;
        [HideInInspector]
        public Rigidbody2D boxRigidbody;
        [HideInInspector]
        Transform pushableBox;
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public float speedOfX;
        [HideInInspector]
        public float speedOfY;
        [HideInInspector]
        public Transform ladderTriggle;
        [HideInInspector]
        public Transform underWaterBottom;

        [HideInInspector]
        public bool isJump = false;
        [HideInInspector]
        public bool isClimb = false;
        [HideInInspector]
        public bool isCrouch = false;
        [HideInInspector]
        public bool dragableOrPushable = false;
        [HideInInspector]
        public bool isPerformSkill = false;
        [HideInInspector]
        public bool isPerformSkilUnderWater = false;
        [HideInInspector]
        public bool isHurt = false;
        [HideInInspector]
        public bool isDead = false;
        [HideInInspector]
        public bool isPassLevel = false;
        [HideInInspector]
        public bool isPlayHurtAnim = false;
        [HideInInspector]
        public bool isGrounded = true;
        [HideInInspector]
        public bool isLadderTop = false;
        [HideInInspector]
        public bool isFacingRight = true;
        [HideInInspector]
        public bool isAttack = false;

        private Transform groundCheck;
        private readonly float groundRadius = 0.1f;
        private Transform ladderCheck;
        private readonly float ladderRadius = 0.6f;
        private Transform interactiveCehck;
        private readonly float interactiveRadius = 0.5f;
        private readonly float boxDropRadius = 0.3f;
        private readonly float waterSideRadius = 0.1f;
        private readonly float offsetOfIceCubeTwo = 0.1f;
        private readonly float offsetOfScaleIceCube = 0.1f;
        private float lengthOfCubeCell;
        private Transform water;
        private Transform lastIceCube;
        private enum IceCubePosition { left, right };
        IceCubePosition placedIceCubePos;

        public void Awake()
        {
            //control = transform.parent.GetComponent<Control>();
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            groundCheck = transform.Find("GroundCheck");
            ladderCheck = transform.Find("LadderCheck");
            interactiveCehck = transform.Find("InteractiveCheck");


            Vector3 left = iceCubeCell.transform.Find("LeftSide").position;
            Vector3 right = iceCubeCell.transform.Find("RightSide").position;
            lengthOfCubeCell = Vector3.Distance(left, right);

            UnderWater.changeStateToSwim += ChangeStateToSwim;
            TouchWater.changeStateToSwim += ChangeStateToSwim;
        }

        public void start()
        {

        }

        public void FixedUpdate()
        {
            StateMachine.StateMacheUpdate();
            animator.SetFloat("SpeedOfX", Mathf.Abs(rigid.velocity.x));
            animator.SetFloat("SpeedOfY", rigid.velocity.y);
            animator.SetBool("Ground", isGrounded);
            animator.SetBool("Jump", isJump);
            animator.SetBool("Attack", isAttack);
            //animator.SetBool("isClimb", isClimb);
            //animator.SetBool("isLadderTop", isLadderTop);
            //animator.SetBool("isCrouch", isCrouch);
            //animator.SetBool("isEnding", isPassLevel);
        }

        public void ChangeStateToSwim()
        {
            StateMachine.ChangeState(PlayerSwinState.Instance);
        }

        public void IsGrounded(string lable = " ")
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, playerInfo.ground);
            
            foreach (var item in colliders)
            {
                if (item != gameObject.GetComponent<Collider2D>() && item.tag != "CheckLadderTop")
                {
                    //Debug.Log(lable + item + "---" + gameObject);
                    isGrounded = true;
                    isLadderTop = false;
                }
            }
        }


        /// <summary>
        /// Check that the down arrow is pushed on condition that the player is on the ground to climb down the ladder.
        /// </summary>
        public void GroundDownToClimb()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, playerInfo.ground);
            foreach (var item in colliders)
            {
                //Debug.Log(item.tag + "---CheckLadderTop");
                if (item.tag == "CheckLadderTop")
                {
                    isLadderTop = true;
                    ladderTriggle = item.transform.parent.Find("LadderTrigger");

                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
                    {
                        Vector2 vec = transform.position;
                        vec.x = ladderTriggle.position.x;
                        vec.y -= 1.5f;
                        transform.position = vec;
                        isClimb = true;
                        // Change the state to climbing
                        StateMachine.ChangeState(PlayerClimbState.Instance);
                    }
                }
            }
        }

        /// <summary>
        /// Check that player is arriving the top of ladder while climbing.
        /// </summary>
        public void IsArriveLadderTop()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, playerInfo.ladder);
            foreach (var item in colliders)
            {
                // Check if player is on the top of ladder
                if (item.tag == "CheckLadderTop")
                {
                    isLadderTop = true;
                    isGrounded = true;
                }
            }
        }
        /// <summary>
        /// Check that player is jumpping to climb ladder.
        /// </summary>
        public void JumpToClimb()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(ladderCheck.position, ladderRadius, playerInfo.ladder);
            foreach (var item in colliders)
            {
                if (item.tag == "LadderTrigger")
                {
                    ladderTriggle = item.transform;

                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
                    {
                        isClimb = true;

                        Vector3 vec = transform.position;
                        vec.x = item.transform.position.x;
                        transform.position = vec;
                    }
                }
            }
        }
        /// <summary>
        /// Check that player's state change to drag or push from idle.
        /// </summary>
        public void IdleToDragOrPush()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactiveCehck.position, interactiveRadius, playerInfo.ground);
            foreach (var item in colliders)
            {
                if (item.name == "PushableBox")
                {
                    //sDebug.Log("close to box");
                    dragableOrPushable = true;
                    pushableBox = item.transform;
                    boxRigidbody = pushableBox.GetComponent<Rigidbody2D>();
                }
            }
        }
        /// <summary>
        /// Check that player stop dragging or pushing.
        /// </summary>
        public void DragOrPushToIdle()
        {
            if (pushableBox != null)
            {
                Transform boxGroundCheck = pushableBox.Find("BoxGroundCheck");
                Collider2D[] colliders = Physics2D.OverlapCircleAll(boxGroundCheck.position, boxDropRadius, playerInfo.ground);
                int id = Array.IndexOf(colliders, GameObject.Find("Ground").GetComponent<Collider2D>());
                if (id == -1 && dragableOrPushable == true)
                {
                    dragableOrPushable = false;
                }
            }
            
        }

        /// <summary>
        /// Check if player approachs to water side.
        /// </summary>
        public void IdleToPerfoemSkill()
        {
            Vector3 direction;
            if (isFacingRight)
                direction = Vector3.right * Mathf.Sqrt(3) + Vector3.down;
            else
                direction = Vector3.left * Mathf.Sqrt(3) + Vector3.down;
            Vector3 endPoint = interactiveCehck.position + direction;

            Debug.DrawRay(interactiveCehck.position, direction, Color.black);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(endPoint, waterSideRadius, playerInfo.water);
            string[] names = new string[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
                names[i] = colliders[i].name;
            int bottomId = Array.IndexOf(names, "Bottom");
            //int iceBlockId = Array.IndexOf(names, "IceBlock");
            // Set trigger of perform skill only "Bottom" been detected.
            if (bottomId != -1 && colliders.Length == 1)
            {
                water = colliders[bottomId].transform;
                isPerformSkill = true;
            }
            else
            {
                isPerformSkill = false;
            }
        }

        /// <summary>
        /// Place the ice block.
        /// </summary>
        public void PlaceIceBlock()
        {
            // compute size of ice cube
            Vector3 left = iceCube.transform.Find("LeftSide").position;
            Vector3 right = iceCube.transform.Find("RightSide").position;
            float lengthOfIceCube = Vector3.Distance(left, right);

            // compute the ancher point of water bottom
            float pixelsPerUnit = water.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            float width = water.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnit;
            float height = water.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnit;
            width *= water.localScale.x;
            height *= water.localScale.y;

            Vector3 leftTop = new Vector3(water.position.x - width / 2, water.position.y + height / 2, water.position.z);
            Vector3 rightTop = new Vector3(water.position.x + width / 2, water.position.y + height / 2, water.position.z);

            float lengthOfScaledIceCube = Vector3.Distance(leftTop, rightTop) / 2;
            float scaleX = lengthOfScaledIceCube / lengthOfIceCube;
            iceCube.transform.localScale = new Vector3(scaleX, 1, 1);

            if (water.Find("IceCube(Clone)") == null)
            {
                if (isFacingRight)
                {
                    Vector3 iceCubeCenter = new Vector3(leftTop.x + (lengthOfScaledIceCube / 2) - offsetOfIceCubeTwo, rightTop.y, 0);
                    LoadIceCube(iceCube, water, iceCubeCenter);
                    placedIceCubePos = IceCubePosition.left;

                }
                else
                {
                    Vector3 iceCubeCenter = new Vector3(rightTop.x - (lengthOfScaledIceCube / 2) + offsetOfIceCubeTwo, rightTop.y, 0);
                    LoadIceCube(iceCube, water, iceCubeCenter);
                    placedIceCubePos = IceCubePosition.right;
                }
            }
            else
            {
                if (placedIceCubePos == IceCubePosition.left)
                {
                    float xCenter = lastIceCube.Find("RightSide").transform.position.x + (lengthOfScaledIceCube / 2) + offsetOfIceCubeTwo;
                    Vector3 iceCubeCenter = new Vector3(xCenter, rightTop.y, 0);
                    LoadIceCube(iceCube, water, iceCubeCenter);
                    placedIceCubePos = IceCubePosition.right;
                }
                else if (placedIceCubePos == IceCubePosition.right)
                {
                    float xCenter = lastIceCube.Find("LeftSide").transform.position.x - (lengthOfScaledIceCube / 2) - offsetOfIceCubeTwo;
                    Vector3 iceCubeCenter = new Vector3(xCenter, rightTop.y, 0);
                    LoadIceCube(iceCube, water, iceCubeCenter);
                    placedIceCubePos = IceCubePosition.left;
                }
            }
            iceCube.transform.localScale = new Vector3(1, 1, 1);
        }

        private void LoadIceCube(GameObject prefab, Transform parent, Vector3 iceCubeCenter)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            go.transform.position = iceCubeCenter;
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            go.transform.parent = parent;
            lastIceCube = go.transform;
        }

        /// <summary>
        /// Check if player is under water.
        /// </summary>
        public void IdleToPerfoemSkillUnderWater()
        {
            Vector3 endPoint = interactiveCehck.position + Vector3.up;
            Debug.DrawRay(interactiveCehck.position, Vector3.up, Color.red);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(endPoint, waterSideRadius, playerInfo.water);
            string[] names = new string[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                names[i] = colliders[i].name;
            }
            int bottomId = Array.IndexOf(names, "Bottom");
            int iceCubeId = Array.IndexOf(names, "IceCubeCell(Clone)");
            
            if (StateMachine.CurrentState == PlayerSwinState.Instance && bottomId != -1 && iceCubeId == -1)
            {
                isPerformSkilUnderWater = true;
                underWaterBottom = colliders[bottomId].transform;
            }
        }

        /// <summary>
        /// Place the ice cube under water.
        /// </summary>
        public void PlaceIceCubeUnderWater()
        {
            Transform leftBottom = underWaterBottom.Find("AncherLeftBottom");
            Transform rightBottom = underWaterBottom.Find("AncherRightBottom");

            float lengthOfIceCube = Vector3.Distance(leftBottom.position, rightBottom.position) / 3;
            float scaleX = lengthOfIceCube / lengthOfCubeCell - offsetOfScaleIceCube;
            iceCubeCell.transform.localScale = new Vector3(scaleX, 1, 1);

            // There are three segment to place the ice cube.
            if (transform.position.x > leftBottom.position.x && transform.position.x < leftBottom.position.x + lengthOfIceCube)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + lengthOfIceCube / 2, underWaterBottom.position.y / 2, 0);
                LoadIceCube(iceCubeCell, underWaterBottom, iceCubeCenter);
            }
            else if (transform.position.x > leftBottom.position.x + lengthOfIceCube && transform.position.x < leftBottom.position.x + 2 * lengthOfIceCube)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + 3 * lengthOfIceCube / 2, underWaterBottom.position.y / 2, 0);
                LoadIceCube(iceCubeCell, underWaterBottom, iceCubeCenter);
            }
            else if (transform.position.x > leftBottom.position.x + 2 * lengthOfIceCube && transform.position.x < rightBottom.position.x)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + 5 * lengthOfIceCube / 2, underWaterBottom.position.y / 2, 0);
                LoadIceCube(iceCubeCell, underWaterBottom, iceCubeCenter);
            }
            iceCubeCell.transform.localScale = new Vector3(1, 1, 1);
        }


        /// <summary>
        /// Control the player move horizontally when the state is "Running State", "Crouching State" or "Swimming State".
        /// </summary>
        public void MoveHorizontally()
        {
            float moveFactor = 1;

            // 
            if (StateMachine.CurrentState == PlayerRunState.Instance || StateMachine.CurrentState == PlayerJumpState.Instance)
            {
                moveFactor = 1;
            }
            else if (StateMachine.CurrentState == PlayerCrouchState.Instance)
            {
                moveFactor = playerInfo.crouchSpeedFactor;
            }
            else if (StateMachine.CurrentState == PlayerSwinState.Instance)
            {
                moveFactor = playerInfo.crouchSpeedFactor;
            }

            if (Input.GetKey(KeyCode.A))
            {
                speedOfX = -playerInfo.maxSpeed * moveFactor;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                speedOfX = playerInfo.maxSpeed * moveFactor;
            }
            else
            {
                speedOfX = 0;
            }

            rigid.velocity = new Vector2(speedOfX, rigid.velocity.y);

            if (speedOfX > 0 && isFacingRight == false)
            {
                HorizontalFlip();
            }
            else if (speedOfX < 0 && isFacingRight == true)
            {
                HorizontalFlip();
            }
        }

        public void MoveWithBox()
        {
            if (pushableBox != null)
            {
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.E))
                {
                    speedOfX = -playerInfo.dragOrPushSpeed;
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E))
                {
                    speedOfX = playerInfo.dragOrPushSpeed;
                }
                else
                {
                    speedOfX = 0;
                }

                rigid.velocity = new Vector2(speedOfX, rigid.velocity.y);
                boxRigidbody.velocity = new Vector2(speedOfX, rigid.velocity.y);
            }
        }

        private void HorizontalFlip()
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            isFacingRight = !isFacingRight;
        }

        public void OnPassLevel()
        {
            rigid.velocity = Vector2.zero;
            //animator.SetFloat("SpeedOfX", 0);
            //animator.SetFloat("SpeedOfY", 0);
            rigid.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }

        // Play hurt animation when the assist player is hurt
        public void OnAssistPlayerBeHurt()
        {
            isPlayHurtAnim = true;
            isHurt = true;
            StartCoroutine(HurtAnim());
        }

        private IEnumerator HurtAnim()
        {
            Vector4 color = GetComponent<SpriteRenderer>().color;
            for (int i = 0; i < 20; i++)
            {
                if (i % 2 == 0)
                    color.w = 0.3f;
                else
                    color.w = 1f;
                GetComponent<SpriteRenderer>().color = color;
                yield return new WaitForSeconds(0.2f);
            }
            StopCoroutine(HurtAnim());
            isPlayHurtAnim = false;
        }
    }
}
