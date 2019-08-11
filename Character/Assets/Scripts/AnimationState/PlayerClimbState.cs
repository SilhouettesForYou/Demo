using System;
using UnityEngine;

namespace Demo
{
    public class PlayerClimbState : MachineBehaviourState<AssistPlayerControl>
    {
        private static PlayerClimbState m_Instance;
        public static PlayerClimbState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerClimbState();
                return m_Instance;
            }
        }

        private PlayerClimbState() { }

        public override void Enter()
        {
            owner.isGrounded = false;
            owner.speedOfY = owner.playerInfo.climbSpeed;
            owner.ladderTriggle.parent.Find("LadderTop").GetComponent<BoxCollider2D>().enabled = false;
        }

        public override void Excute()
        {
            owner.rigid.constraints |= RigidbodyConstraints2D.FreezePositionY;

            owner.IsGrounded();
            owner.IsArriveLadderTop();
    
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector2 vec = owner.transform.position;
                vec.y += Time.deltaTime * owner.playerInfo.climbSpeed;
                owner.transform.position = vec;
                owner.speedOfY = owner.playerInfo.climbSpeed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Vector2 vec = owner.transform.position;
                vec.y -= Time.deltaTime * owner.playerInfo.climbSpeed;
                owner.transform.position = vec;
                owner.speedOfY = owner.playerInfo.climbSpeed;
            }
            else
            {
                owner.speedOfY = 0;
            }

            if (owner.isGrounded)
            {
                owner.rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
        }

        public override void Exit()
        {
            owner.speedOfY = 0;
            owner.isClimb = false;
            owner.ladderTriggle.parent.Find("LadderTop").GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
