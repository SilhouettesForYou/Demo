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
            owner.CheckPerformSkillUnderWater();

            if (owner.isAttack)
            {
                owner.StateMachine.ChangeState(PlayerAttackState.Instance);
            }

            
            if (InputManager.JumpBtnDown)
            {
                owner.StateMachine.ChangeState(PlayerJumpState.Instance);
            }
            else if (!InputManager.LeftBtnDown && !InputManager.RightBtnDown && !owner.isDropping)
            {
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
            else if (owner.isDead)
            {
                owner.StateMachine.ChangeState(PlayerDeadState.Instance);
            }
            else if (owner.isCloseToPushable == true && InputManager.InteractiveBtnDown)
            {
                owner.StateMachine.ChangeState(PlayerPushGragState.Instance);
            }
            else if (owner.isPerformSkilUnderWater == true && InputManager.SkillBtnDown)
            {
                owner.StateMachine.ChangeState(PerformAbilitiesState.Instance);
            }
        }

        public override void Exit()
        {

        }
    }
}
