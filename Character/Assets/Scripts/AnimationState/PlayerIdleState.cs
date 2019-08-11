using System.Collections;
using UnityEngine;

namespace Demo
{
    public class PlayerIdleState : MachineBehaviourState<AssistPlayerControl>
    {
        private static PlayerIdleState m_Instance;
        public static PlayerIdleState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerIdleState();
                return m_Instance;
            }
        }

        private PlayerIdleState() { }

        public override void Enter()
        {
            
        }

        public override void Excute()
        {
            // Check if player is going to climb down on the ground above the ladder
            owner.GroundDownToClimb();
            // check if player is on the ground
            owner.IsGrounded();
            // check if player is going to do the movement of dragging or pushing
            owner.IdleToDragOrPush();
            // check if player is going to perfoem the skill
            owner.IdleToPerfoemSkill();
            // change state
            if (!owner.isPassLevel)
            {
                if (owner.dragableOrPushable == true && Input.GetKey(KeyCode.E))
                {
                    owner.StateMachine.ChangeState(PlayerPushGragState.Instance);
                }
                else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                    Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
                {
                    owner.StateMachine.ChangeState(PlayerRunState.Instance);
                }
                else if (owner.isLadderTop == false && (Input.GetKey(KeyCode.Space))) // climb up
                {
                    //Debug.Log("Change to jumpping state");
                    owner.StateMachine.ChangeState(PlayerJumpState.Instance);
                }
                else if (owner.isLadderTop == false && (Input.GetKey(KeyCode.S))) // climb down
                {
                    owner.StateMachine.ChangeState(PlayerCrouchState.Instance);
                }
                else if (owner.isPerformSkill == true && Input.GetKeyDown(KeyCode.E))
                {
                    owner.StateMachine.ChangeState(PerformAbilitiesState.Instance);
                }
            }
        }

        public override void Exit()
        {
            
        }
    }
}
