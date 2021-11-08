using UnityEngine;

namespace Demo
{
    public class PlayerJumpState : MachineBehaviourState<Pankapu>
    {
        private float gravity;

        private static PlayerJumpState m_Instance;
        public static PlayerJumpState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerJumpState();
                return m_Instance;
            }
        }
        public override void Enter()
        {
            gravity = owner.rigid.gravityScale;
            if (owner.StateMachine.PreState == PlayerAttackState.Instance)
            {
                owner.rigid.AddForce((owner.config.jumpForce / 10) * Vector2.down);
            }
            else
            {
                //owner.rigid.AddForce(owner.config.jumpForce * Vector2.up);
                //owner.rigid.velocity = Vector2.up * owner.config.jumpVelocity;
                owner.StartCoroutine(owner.JumpRoutine());
                owner.isJump = true;
                owner.isGrounded = false;
            }
            
        }

        public override void Excute()
        {
            // check ground
            owner.IsGrounded();
            owner.IsArriveLadderTop();
            owner.IsAttack();
            //BetterJump(owner.rigid);
            if (owner.isAttack)
            {
                owner.StateMachine.ChangeState(PlayerAttackState.Instance);
            }

            if (owner.isGrounded)
            {
                //Debug.Log("Jumpping");
                owner.isJump = false;
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
            else if (owner.isDead)
            {
                owner.StateMachine.ChangeState(PlayerDeadState.Instance);
            }
            else
            {
                if (InputManager.LeftBtnDown || InputManager.RightBtnDown)
                {
                    owner.MoveHorizontallyInAir();
                }
            }

            // check ladder
            owner.JumpToClimb();
            if (owner.isClimb)
            {
                owner.StateMachine.ChangeState(PlayerClimbState.Instance);
            }
        }

        public override void Exit()
        {
            owner.rigid.gravityScale = gravity;
            //owner.rigid.Sleep();
            owner.isJump = false;
        }
    }
}
