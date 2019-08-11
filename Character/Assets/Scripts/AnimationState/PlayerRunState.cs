using UnityEngine;

namespace Demo
{
    public class PlayerRunState : MachineBehaviourState<AssistPlayerControl>
    {
        private static PlayerRunState m_Instance;
        public static PlayerRunState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerRunState();
                return m_Instance;
            }
        }

        private PlayerRunState() { }

        public override void Enter()
        {

        }

        public override void Excute()
        {
            owner.MoveHorizontally();
            // change state
            if (Mathf.Abs(owner.speedOfX) < 0.1f || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                owner.StateMachine.ChangeState(PlayerJumpState.Instance);
            }
            else if (owner.isLadderTop = false && Input.GetKey(KeyCode.S))
            {
                owner.StateMachine.ChangeState(PlayerCrouchState.Instance);
            }
        }

        public override void Exit()
        {

        }
    }
}
