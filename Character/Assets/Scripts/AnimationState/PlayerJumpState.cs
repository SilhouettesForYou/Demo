using UnityEngine;

namespace Demo
{
    public class PlayerJumpState : MachineBehaviourState<AssistPlayerControl>
    {
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
            if (owner.StateMachine.PreState == PlayerAttackState.Instance)
            {
                owner.rigid.AddForce((owner.playerInfo.jumpForce / 10) * Vector2.down);
            }
            else
            {
                owner.rigid.AddForce(owner.playerInfo.jumpForce * Vector2.up);
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

            if (owner.isAttack)
            {
                owner.StateMachine.ChangeState(PlayerAttackState.Instance);
            }

            if (owner.isGrounded)
            {
                //Debug.Log("Jumpping");
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    owner.MoveHorizontally();
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
            owner.rigid.Sleep();
            owner.isJump = false;
        }
    }
}
