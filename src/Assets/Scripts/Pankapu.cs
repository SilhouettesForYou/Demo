using System;
using System.Collections;
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

        public PankapuConfig config;
        public GameObject iceCube;
        public GameObject iceCubePool;
        public AnimationCurve jumpCurve;

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
        public bool isRespawn = true;
        [HideInInspector]
        public bool isAnDead = false;
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
        public bool isPrePushing = false;
        [HideInInspector]
        public bool isPushing = false;
        [HideInInspector]
        public bool isMoving = false;
        [HideInInspector]
        public bool isDropping = false;
        [HideInInspector]
        public bool push = false;
        [HideInInspector]
        public bool drag = false;
        [HideInInspector]
        public bool isReleaseBox = true;
        [HideInInspector]
        public bool frezeeAttack = false;
        [HideInInspector]
        public bool frezeeAll = false;
        [HideInInspector]
        public bool isCloseToPushable = false;
        [HideInInspector]
        public bool isRespawnAfterAn = false;

        //[HideInInspector]
        //public Light2D frezeeLightLeft;
        //[HideInInspector]
        //public Light2D frezeeLightRight;
        private Transform groundCheck;
        private readonly float groundRadius = 0.2f;
        private Transform ladderCheck;
        private readonly float ladderRadius = 0.6f;
        private Transform interactiveCheck;
        private readonly float interactiveRadius = 1.0f;
        private readonly float waterSideRadius = 0.1f;
        private readonly float offsetOfIceCubeTwo = 0.0f;
        private readonly float offsetOfScaleIceCube = 0.1f;
        private int leftIceCube = 1;
        private int middleIceCube = 1;
        private int rightIceCube = 1;
        private int littleLeftIceCube = 1;
        private int littleRightIceCube = 1;
        private Transform water;

        private static CheckPoint lastCheckPoint = null;
        private Vector3 startPosition;
        private float capsuleWith;

        private enum IceCubePosition { left, right };
        IceCubePosition placedIceCubePos;

        void Awake()
        {
            Scene scene = SceneManager.GetActiveScene();
            string curSceneName = scene.name;
            if (curSceneName == "Level-1")
            {
                frezeeAttack = true;
                //frezeeLightLeft = transform.Find("FrezeeLightLeft").GetComponent<Light2D>();
                //frezeeLightRight = transform.Find("FrezeeLightRight").GetComponent<Light2D>();
            }
            else
            {
                frezeeAttack = false;
            }

            //control = transform.parent.GetComponent<Control>();
            rigid = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            groundCheck = transform.Find("GroundCheck");
            ladderCheck = transform.Find("LadderCheck");
            interactiveCheck = transform.Find("InteractiveCheck");
            startPosition = FindObjectOfType<An>().transform.position;

            playerMass = rigid.mass;

            capsuleWith = GetComponent<CapsuleCollider2D>().size.x * transform.localScale.x;
        }

        void Start()
        {
            // Add listener
            EventCenter.AddListener(EventType.PankapuDeath, CheckDead);
            EventCenter.AddListener(EventType.AnDeath, CheckAnDead);
            //EventCenter.AddListener<bool>(EventType.TouchWater, ChangeStateToSwim);
            EventCenter.AddListener(EventType.Dive, ChageStateToDive);
            EventCenter.AddListener<bool>(EventType.FrezeeAll, IsFrezeeAll);
            EventCenter.AddListener<Transform>(EventType.DivingInPool, IdleToPerfoemSkillUnderWater);
            EventCenter.AddListener(EventType.AnRespawn, Respawn);
        }      

        public void FixedUpdate()
        {
            IsAttack();
            StateMachine.StateMacheUpdate();
            if (InputManager.RightBtnDown || InputManager.LeftBtnDown)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            if (rigid.velocity.y < -0.2 && !isJump)
            {
                isDropping = true;
                rigid.AddForce(Vector2.down * 25);
            }
            else
            {
                isDropping = false;
            }
            animator.SetBool("Move", isMoving);
            animator.SetBool("Drop", isDropping);
            animator.SetFloat("SpeedOfY", rigid.velocity.y);
            animator.SetBool("Ground", isGrounded);
            animator.SetBool("PrePushDrag", isPrePushing);
            animator.SetBool("Jump", isJump);
            animator.SetBool("Push", push);
            animator.SetBool("Drag", drag);
            animator.SetBool("Attack", isAttack);
            animator.SetBool("Dead", isDead);
            animator.SetBool("Respawn", isRespawn);

            if (frezeeAll)
            {
                rigid.velocity = new Vector2(0, 0);
            }
            
            //animator.SetBool("isClimb", isClimb);
            //animator.SetBool("isLadderTop", isLadderTop);
            //animator.SetBool("isCrouch", isCrouch);
            //animator.SetBool("isEnding", isPassLevel);
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener(EventType.PankapuDeath, CheckDead);
            EventCenter.RemoveListener(EventType.AnDeath, CheckAnDead);
            //EventCenter.AddListener<bool>(EventType.TouchWater, ChangeStateToSwim);
            EventCenter.RemoveListener(EventType.Dive, ChageStateToDive);
            EventCenter.RemoveListener<bool>(EventType.FrezeeAll, IsFrezeeAll);
            EventCenter.RemoveListener<Transform>(EventType.DivingInPool, IdleToPerfoemSkillUnderWater);
            EventCenter.RemoveListener(EventType.AnRespawn, Respawn);
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

        public static void SetCheckPoint(CheckPoint checkpoint)
        {
            lastCheckPoint = checkpoint;
        }

        private void UpdateFacing()
        {
            if (!isFacingRight)
            {
                HorizontalFlip();
            }
        }

        public void Respawn()
        {
            if (lastCheckPoint != null)
            {
                UpdateFacing();
                transform.position = lastCheckPoint.transform.position;
            }
            else
            {
                UpdateFacing();
                transform.position = startPosition;
            }
            isAnDead = false;
            isPrePushing = false;
            isRespawn = true;
            leftIceCube = 1;
            rightIceCube = 1;
            middleIceCube = 1;
            EventCenter.Braodcast<bool>(EventType.FrezeeAll, false);
            StateMachine.ChangeState(PlayerIdleState.Instance);
        }

        public void IsGrounded()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius,
                config.ground | config.pushable | config.movingPlatform | config.water);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform.GetComponent<Ground>() != null ||
                    colliders[i].transform.GetComponent<Pushable>() != null ||
                    colliders[i].transform.GetComponent<IceBlock>() != null)
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }

            //colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, config.water);

            //foreach (var item in colliders)
            //{
            //    if (item != gameObject.GetComponent<Collider2D>())
            //    {

            //    }
            //}
        }

        private float Curve(float x)
        {
            return Mathf.Pow((x - config.jumpTime / 2), 2) * Mathf.Pow(config.jumpTime / 2, -2);
        }

        public IEnumerator JumpRoutine()
        {
            float time = 0.0f;
            while (time < config.jumpTime)
            {
                float velocity = Curve(time) * config.jumpForce;
                rigid.velocity = new Vector2(rigid.velocity.x, velocity);
                if (time > config.jumpTime / 2)
                    rigid.gravityScale = 5f;
                time += config.jumpTime / config.jumpVelocity;
            }
            yield return null;
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

        public string CloseEnoughToPushable()
        {
            string name = "";
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactiveCheck.position, interactiveRadius, config.pushable);
            foreach (var collider in colliders)
            {
                //Debug.Log(collider.transform.name);
                Pushable pushable = collider.transform.GetComponent<Pushable>();
                if (pushable != null)
                {
                    isCloseToPushable = true;
                    name = pushable.transform.name;
                }
            }
            if (colliders.Length == 0)
            {
                isCloseToPushable = false;
            }
            if (isAnDead)
                isCloseToPushable = false;
            return name;
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
                direction = Vector3.right * Mathf.Sqrt(3) * 0.5f + Vector3.down;
            else
                direction = Vector3.left * Mathf.Sqrt(3) * 0.5f + Vector3.down;
            Vector3 endPoint = interactiveCheck.position + direction * 2.0f;

            Debug.DrawLine(interactiveCheck.position, endPoint, Color.red);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(endPoint, waterSideRadius, config.water);
            string[] names = new string[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                names[i] = colliders[i].name;
            }
            int bottomId = Array.IndexOf(names, "Bottom");
            //int iceBlockId = Array.IndexOf(names, "IceBlock");
            // Set trigger of perform skill only "Bottom" been detected.
            if (bottomId != -1 && colliders.Length == 1 
                && Array.IndexOf(names, "IceCube(Clone)") == -1
                && Array.IndexOf(names, "PushableIceCube") == -1)
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
            if (water.GetComponent<WaterFalls>() != null)
            {
                Transform leftBottom = water.Find("AncherLeftTop");
                Transform rightBottom = water.Find("AncherRightTop");
                float lengthOfCubeCell = PankapuConfig.ComputeRect(iceCube.transform).x;
                float lengthOfIceCube = Vector3.Distance(leftBottom.position, rightBottom.position);
                float scaleX = lengthOfIceCube / lengthOfCubeCell - offsetOfScaleIceCube;
                iceCube.transform.localScale = new Vector3(scaleX, 0.5f, 1);

                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + lengthOfIceCube / 2, leftBottom.position.y, 0);
                LoadIceCube(iceCube, water, iceCubeCenter);
                iceCube.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                // compute size of ice cube
                float lengthOfIceCube = PankapuConfig.ComputeRect(iceCube.transform).x;

                // compute the ancher point of water bottom
                float width = PankapuConfig.ComputeRect(water).x * water.parent.localScale.x;
                float height = PankapuConfig.ComputeRect(water).y * water.parent.localScale.y;

                Vector3 leftTop = new Vector3(water.position.x - width / 2, water.position.y + height / 2, water.position.z);
                Vector3 rightTop = new Vector3(water.position.x + width / 2, water.position.y + height / 2, water.position.z);

                float lengthOfScaledIceCube = Vector3.Distance(leftTop, rightTop) / 2;
                float scaleX = lengthOfScaledIceCube / lengthOfIceCube;
                iceCube.transform.localScale = new Vector3(scaleX, 0.3f, 1);

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
                        float xCenter = rightTop.x - (lengthOfScaledIceCube / 2) + offsetOfIceCubeTwo;
                        Vector3 iceCubeCenter = new Vector3(xCenter, rightTop.y, 0);
                        LoadIceCube(iceCube, water, iceCubeCenter);
                        placedIceCubePos = IceCubePosition.right;
                    }
                    else if (placedIceCubePos == IceCubePosition.right)
                    {
                        float xCenter = leftTop.x + (lengthOfScaledIceCube / 2) - offsetOfIceCubeTwo;
                        Vector3 iceCubeCenter = new Vector3(xCenter, rightTop.y, 0);
                        LoadIceCube(iceCube, water, iceCubeCenter);
                        placedIceCubePos = IceCubePosition.left;
                    }
                }
                iceCube.transform.localScale = new Vector3(1, 1, 1);
            }
            
        }

        private void LoadIceCube(GameObject prefab, Transform parent, Vector3 iceCubeCenter)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            go.transform.position = iceCubeCenter;
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            go.transform.parent = parent;
        }

        /// <summary>
        /// Check if player is under water.
        /// </summary>
        public void IdleToPerfoemSkillUnderWater(Transform t)
        {
            underWaterBottom = t;
        }

        public void CheckPerformSkillUnderWater()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(interactiveCheck.position, Vector2.up * 5.0f,
                config.water);
            foreach (var hit in hits)
            {
                if (hit.transform != null && hit.transform.GetComponent<IceBlock>() != null)
                {
                    isPerformSkilUnderWater = false;
                }
                else if (hit.transform != null && hit.transform.GetComponent<UnderWater>() != null)
                {
                    isPerformSkilUnderWater = true;
                }
            }
        }

        /// <summary>
        /// Place the ice cube under water.
        /// </summary>
        public void PlaceIceCubeUnderWater()
        {
            Transform leftBottom = underWaterBottom.Find("AncherLeftBottom");
            Transform rightBottom = underWaterBottom.Find("AncherRightBottom");
            float lengthOfCubeCell= PankapuConfig.ComputeRect(iceCubePool.transform).x;
            float lengthOfIceCube = Vector3.Distance(leftBottom.position, rightBottom.position) / 3;
            float scaleX = lengthOfIceCube / lengthOfCubeCell;
            iceCubePool.transform.localScale = new Vector3(scaleX, 0.5f, 1);

            // There are three segment to place the ice cube.
            if (transform.position.x > leftBottom.position.x && transform.position.x < leftBottom.position.x + lengthOfIceCube && leftIceCube == 1)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + lengthOfIceCube / 2, ladderCheck.position.y, 0);
                LoadIceCube(iceCubePool, underWaterBottom, iceCubeCenter);
                leftIceCube--;
            }
            else if (transform.position.x > leftBottom.position.x + lengthOfIceCube && transform.position.x < leftBottom.position.x + 2 * lengthOfIceCube && middleIceCube == 1)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + 3 * lengthOfIceCube / 2, ladderCheck.position.y, 0);
                LoadIceCube(iceCubePool, underWaterBottom, iceCubeCenter);
                middleIceCube--;
            }
            else if (transform.position.x > leftBottom.position.x + 2 * lengthOfIceCube && transform.position.x < rightBottom.position.x && rightIceCube == 1)
            {
                Vector3 iceCubeCenter = new Vector3(leftBottom.position.x + 5 * lengthOfIceCube / 2, ladderCheck.position.y, 0);
                LoadIceCube(iceCubePool, underWaterBottom, iceCubeCenter);
                rightIceCube--;
            }
            iceCubePool.transform.localScale = new Vector3(1, 1, 1);
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
            Vector3 swordOffset = new Vector3(interactiveCheck.position.x + (isFacingRight ? 1 : -1) * 0.5f,
                interactiveCheck.position.y, interactiveCheck.position.z);
            GameObject go = Instantiate(Resources.Load("Prefabs/ShockWave")) as GameObject;
            go.transform.parent = transform;
            go.transform.position = swordOffset;
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (isFacingRight ? 0 : 180)));
            go.transform.GetComponent<Rigidbody2D>().AddForce(Vector3.right * (isFacingRight ? 1 : -1) * 500);
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
            SpeedUpOnOil speedUpOnOil = GetComponent<SpeedUpOnOil>();

            if (speedUpOnOil != null && speedUpOnOil.isOnOil())
            {
                Debug.Log("speed up");
                speedOfX *= speedUpOnOil.speedupRate;
            }

   

            rigid.velocity = new Vector2(speedOfX, rigid.velocity.y);
            //rigid.MovePosition(new Vector2(rigid.position.x + Time.deltaTime * speedOfX, rigid.position.y));

            if (speedOfX > 0 && isFacingRight == false)
            {
                HorizontalFlip();
            }
            else if (speedOfX < 0 && isFacingRight == true)
            {
                HorizontalFlip();
            }
        }


        private bool CheckCollisionWall(Transform start, Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(start.position,
                    direction, capsuleWith / 2 + groundRadius, config.ground | config.pushable);
            if (hit.collider != null)
            {
                if (hit.collider.transform.GetComponent<Ground>() != null ||
                    hit.collider.transform.GetComponent<Pushable>() != null)
                    return true;
            }
            return false;
        }
        public void MoveHorizontallyInAir()
        {
            
            float speedOfY = rigid.velocity.y;
            if (InputManager.LeftBtnDown)
            {
                speedOfX = -config.walkSpeed;
                
                if (CheckCollisionWall(interactiveCheck, Vector2.left) ||
                    CheckCollisionWall(groundCheck, Vector2.left) ||
                    CheckCollisionWall(ladderCheck, Vector2.left))
                {
                    //rigid.AddForce(config.jumpForce * Vector2.up);
                    speedOfY = 10f;
                }
            }
            else if (InputManager.RightBtnDown)
            {
                speedOfX = config.walkSpeed;
                if (CheckCollisionWall(interactiveCheck, Vector2.right) ||
                    CheckCollisionWall(groundCheck, Vector2.right) ||
                    CheckCollisionWall(ladderCheck, Vector2.right))
                {
                    //rigid.AddForce(config.jumpForce * Vector2.up);
                    speedOfY = 10f;
                }
            }
            else if (InputManager.LeftBtnUp || InputManager.RightBtnUp)
            {
                speedOfX = 0;
            }

            rigid.velocity = new Vector2(speedOfX, speedOfY);
            //rigid.MovePosition(new Vector2(rigid.position.x + Time.deltaTime * speedOfX, rigid.position.y));
            //transform.Translate(new Vector2(rigid.position.x + Time.deltaTime * speedOfX, rigid.position.y));

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
                if (isFacingRight)
                {
                    drag = true;
                    push = false;
                }
                else
                {
                    drag = false;
                    push = true;
                }
            }
            else if (InputManager.RightBtnDown)
            {
                speedOfX = config.pushSpeed;
                if (!isFacingRight)
                {
                    drag = true;
                    push = false;
                }
                else
                {
                    drag = false;
                    push = true;
                }
            }
            else
            {
                speedOfX = 0;
                drag = false;
                push = false;
            }

            rigid.velocity = new Vector2(speedOfX * 1f, rigid.velocity.y);

        }


        private void HorizontalFlip()
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            isFacingRight = !isFacingRight;
        }

        private void CheckDead()
        {
            isDead = true;
            isRespawn = false;
            StateMachine.ChangeState(PlayerDeadState.Instance);
        }

        private void CheckAnDead()
        {
            isAnDead = true;
            //EventCenter.Braodcast<bool>(EventType.FrezeeAll, true);
            //StateMachine.ChangeState(PlayerIdleState.Instance);
        }


        public void OnPassLevel()
        {
            rigid.velocity = Vector2.zero;
            //animator.SetFloat("SpeedOfX", 0);
            //animator.SetFloat("SpeedOfY", 0);
            rigid.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }

    }
}
