using UnityEngine;

namespace Demo
{
    public class PlayerRunState : MachineBehaviourState<Pankapu>
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
            owner.CloseEnoughToPushable();
            owner.IsAttack();

            if (owner.isAttack)
            {
                owner.StateMachine.ChangeState(PlayerAttackState.Instance);
            }

            
            if (InputManager.JumpBtnDown)
            {
                owner.StateMachine.ChangeState(PlayerJumpState.Instance);
            }
            else if (Mathf.Abs(owner.speedOfX) < 0.1f || InputManager.LeftBtnDown || InputManager.RightBtnDown)
            {
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
            else if (owner.isLadderTop = false && Input.GetKey(KeyCode.S))
            {
                owner.StateMachine.ChangeState(PlayerCrouchState.Instance);
            }
            else if (owner.isCloseToPushable == true && InputManager.InteractiveBtnDown)
            {
                owner.StateMachine.ChangeState(PlayerPushGragState.Instance);
            }
        }

        public override void Exit()
        {

        }
    }
}
