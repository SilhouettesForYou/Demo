using System.Collections;
using UnityEngine;

namespace Demo
{
    public class PlayerIdleState : MachineBehaviourState<Pankapu>
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
            owner.rigid.velocity = Vector2.zero;
        }

        public override void Excute()
        {
            // check if player is on the ground.
            owner.IsGrounded();
            // check if player is going to do the movement of dragging or pushing.
            owner.CloseEnoughToPushable();
            // check if player is going to perform the skill.
            owner.IdleToPerfoemSkill();
            // check if player is going to perform the attack skill.
            owner.IsAttack();
            //Debug.Log("IDLE: " + owner.isCloseToPushable);
            // change state
            if (!owner.isPassLevel)
            {
                if (owner.isAttack)
                {
                    owner.StateMachine.ChangeState(PlayerAttackState.Instance);
                }
                if (owner.isCloseToPushable && InputManager.InteractiveBtnDown)
                {
                    owner.StateMachine.ChangeState(PlayerPushGragState.Instance);
                }
                else if (InputManager.LeftBtnDown || InputManager.RightBtnDown)
                {
                    owner.StateMachine.ChangeState(PlayerRunState.Instance);
                }
                else if (InputManager.JumpBtnDown) // climb up
                {
                    //Debug.Log("Change to jumpping state");
                    owner.StateMachine.ChangeState(PlayerJumpState.Instance);
                }
                else if (owner.isPerformSkill == true && InputManager.SkillBtnDown)
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
