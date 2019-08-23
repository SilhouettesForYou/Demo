using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Demo
{
    public class Pankapu : MonoBehaviour
    {
        private StateMachine<Pankapu> m_StateMachine;
        public StateMachine<Pankapu> StateMachine
        { 
            get
            {
                if (m_StateMachine == null)
                    m_StateMachine = new StateMachine<Pankapu>(this, PlayerIdleState.Instance, PlayerGlobalState.Instance);
                return m_StateMachine;
            }
        }

        private Scene curScence;
        public PankapuConfig config;
        public GameObject iceCube;

        //[HideInInspector]
        //public Control control;
        [HideInInspector]
        public Rigidbody2D rigid;
        [HideInInspector]
        public Rigidbody2D boxRigid;
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public float speedOfX;
        [HideInInspector]
        public float speedOfY;
        [HideInInspector]
        public float playerMass;
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
        public bool isPerformSkill = false;
        [HideInInspector]
        public bool isPerformSkilUnderWater = false;
        [HideInInspector]
        public bool isDead = false;
        [HideInInspector]
        public bool isPassLevel = false;
        [HideInInspector]
        public bool isGrounded = true;
        [HideInInspector]
        public bool isInWater = false;
        [HideInInspector]
        public bool isDiving = false;
        [HideInInspector]
        public bool isLadderTop = false;
        [HideInInspector]
        public bool isFacingRight = true;
        [HideInInspector]
        public bool isAttack = false;
        [HideInInspector]
        public bool isPushing = false;
        [HideInInspector]
        public bool isReleaseBox = true;
        [HideInInspector]
        public bool frezeeAttack = false;
        [HideInInspector]
        public bool frezeeAll = false;
        [HideInInspector]
        public bool isCloseToPushable = false;

        [HideInInspector]
        public static float width;
        public static float height;

        private Transform groundCheck;
        private readonly float groundRadius = 0.1f;
        private Transform ladderCheck;
        private readonly float ladderRadius = 0.6f;
        private Transform interactiveCheck;
        private readonly float interactiveRadius = 1.0f;
        private readonly float waterSideRadius = 0.1f;
        private readonly float offsetOfIceCubeTwo = 0.1f;
        private readonly float offsetOfScaleIceCube = 0.1f;
        private float lengthOfCubeCell;
        private Transform water;
        private Transform lastIceCube;
        private enum IceCubePosition { left, right };
        IceCubePosition placedIceCubePos;

        void Awake()
        {
            //control = transform.parent.GetComponent<Control>();
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            groundCheck = transform.Find("GroundCheck");
            ladderCheck = transform.Find("LadderCheck");
            interactiveCheck = transform.Find("InteractiveCheck");


            Vector3 left = iceCube.transform.Find("LeftSide").position;
            Vector3 right = iceCube.transform.Find("RightSide").position;
            lengthOfCubeCell = Vector3.Distance(left, right);

            playerMass = rigid.mass;

            ComputeRect();
        }

        void Start()
        {
            // Add listener
            EventCenter.AddListener(EventType.IsAnPrimeDead, CheckAnPrimeDead);
            EventCenter.AddListener<bool>(EventType.TouchWater, ChangeStateToSwim);
            EventCenter.AddListener(EventType.Dive, ChageStateToDive);
            EventCenter.AddListener<bool>(EventType.FrezeeAttack, IsFrezeeAttack);
            EventCenter.AddListener<bool>(EventType.FrezeeAll, IsFrezeeAll);
        }      

        public void FixedUpdate()
        {
            IsAttack();
            StateMachine.StateMacheUpdate();
            animator.SetFloat("SpeedOfX", Mathf.Abs(rigid.velocity.x));
            animator.SetFloat("SpeedOfY", rigid.velocity.y);
            animator.SetBool("Ground", isGrounded);
            animator.SetBool("Jump", isJump);
            animator.SetBool("Push", isPushing);
            animator.SetBool("Attack", isAttack);
            animator.SetBool("Push", isPushing);
            
            if (frezeeAll)
            {
                rigid.velocity = new Vector2(0, 0);
                StateMachine.ChangeState(PlayerIdleState.Instance);

            }
            //animator.SetBool("isClimb", isClimb);
            //animator.SetBool("isLadderTop", isLadderTop);
            //animator.SetBool("isCrouch", isCrouch);
            //animator.SetBool("isEnding", isPassLevel);
        }
        private void ComputeRect()
        {
            float pixelsPerUnit = transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            width = transform.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnit;
            height = transform.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnit;
            width *= transform.localScale.x;
            height *= transform.localScale.y;
        }

        public void ChangeStateToSwim(bool flag)
        {
            isInWater = flag;
            if (isInWater)
                StateMachine.ChangeState(PlayerSwinState.Instance);
        }

        public void ChageStateToDive()
        {
            isDiving = true;
            StateMachine.ChangeState(PlayerSwinState.Instance);
        }

        public void IsGrounded()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, config.ground);
            
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, 
                1 << LayerMask.NameToLayer("Ground"));
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius,
                1 << LayerMask.NameToLayer("Ladder"));
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(ladderCheck.position, ladderRadius,
                1 << LayerMask.NameToLayer("Ladder"));
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

        public void CloseEnoughToPushable()
        {
            bool isExist = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactiveCheck.position, interactiveRadius, config.ground);
            foreach (var collider in colliders)
            {
                Pushable pushable = collider.transform.GetComponent<Pushable>();
                if (pushable != null)
                {
                    isExist = true;
                }
            }

            isCloseToPushable = isExist ? true : false;

        }

        /// <summary>
        /// Check that player's state change to drag or push from idle.
        /// </summary>

        public void CheckReleaseBox()
        {
            if (!InputManager.InteractiveBtnDown || !isCloseToPushable)
            {
                isReleaseBox = true;
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
            Vector3 endPoint = interactiveCheck.position + direction;

            Debug.DrawRay(interactiveCheck.position, direction, Color.black);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(endPoint, waterSideRadius,
                1 << LayerMask.NameToLayer("Water"));
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
            Vector3 endPoint = interactiveCheck.position + Vector3.up;
            Debug.DrawRay(interactiveCheck.position, Vector3.up, Color.red);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(endPoint, waterSideRadius,
                1 << LayerMask.NameToLayer("Water"));
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
            iceCube.transform.localScale = new Vector3(scaleX, 1, 1);

            // There are three segment to place the ice cube.
            if (transform.position.x > leftBottom.position.x && transform.position.x < leftBottom.position.x + lengthOfIceCube)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + lengthOfIceCube / 2, underWaterBottom.position.y / 2, 0);
                LoadIceCube(iceCube, underWaterBottom, iceCubeCenter);
            }
            else if (transform.position.x > leftBottom.position.x + lengthOfIceCube && transform.position.x < leftBottom.position.x + 2 * lengthOfIceCube)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + 3 * lengthOfIceCube / 2, underWaterBottom.position.y / 2, 0);
                LoadIceCube(iceCube, underWaterBottom, iceCubeCenter);
            }
            else if (transform.position.x > leftBottom.position.x + 2 * lengthOfIceCube && transform.position.x < rightBottom.position.x)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + 5 * lengthOfIceCube / 2, underWaterBottom.position.y / 2, 0);
                LoadIceCube(iceCube, underWaterBottom, iceCubeCenter);
            }
            iceCube.transform.localScale = new Vector3(1, 1, 1);
        }

        private void IsFrezeeAttack(bool flag)
        {
            frezeeAttack = flag;
        }

        private void IsFrezeeAll(bool flag)
        {
            frezeeAll = flag;
        }

        /// <summary>
        /// Check if player perform attack skill.
        /// </summary>
        public void IsAttack()
        {
            if (InputManager.SkillBtnDown && !frezeeAttack)
            {
                isAttack = true;
            }
        }
        /// <summary>
        /// Check if player perform attack skill.
        /// </summary>
        public void FireInTheHole()
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/ShockWave")) as GameObject;
            go.transform.parent = transform;
            go.transform.position = interactiveCheck.position;
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            go.transform.GetComponent<Rigidbody2D>().AddForce(Vector3.right * (isFacingRight ? 1 : -1) * 100);
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
            else if (StateMachine.CurrentState == PlayerSwinState.Instance)
            {
                moveFactor = config.swimSpeed / config.walkSpeed;
            }

            if (InputManager.LeftBtnDown)
            {
                speedOfX = -config.walkSpeed * moveFactor;
            }
            else if (InputManager.RightBtnDown)
            {
                speedOfX = config.walkSpeed * moveFactor;
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
            if (InputManager.LeftBtnDown)
            {
                speedOfX = -config.pushSpeed;
            }
            else if (InputManager.RightBtnDown)
            {
                speedOfX = config.pushSpeed;
            }
            else
            {
                speedOfX = 0;
            }

            rigid.velocity = new Vector2(speedOfX * 1f, rigid.velocity.y);

        }


        private void HorizontalFlip()
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            isFacingRight = !isFacingRight;
        }

        private void CheckAnPrimeDead()
        {
            isDead = true;
        }

        #region Input Manager
        
        #endregion

        public void OnPassLevel()
        {
            rigid.velocity = Vector2.zero;
            //animator.SetFloat("SpeedOfX", 0);
            //animator.SetFloat("SpeedOfY", 0);
            rigid.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }

        // Play hurt animation when the assist player is hurt
        public void OnAssistPlayerBeHurtToDead()
        {
            Debug.Log("An' is dead, game over...");
            StopAllCoroutines();
        }
    }
}
