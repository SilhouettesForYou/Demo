using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PlayerSwinState : MachineBehaviourState<Pankapu>
    {

        private static PlayerSwinState m_Instance;
        public static PlayerSwinState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerSwinState();
                return m_Instance;
            }
        }
        public override void Enter()
        {

        }

        public override void Excute()
        {
            owner.MoveHorizontally();
            owner.CheckPerformSkillUnderWater();
            // check if player is going to perform the skill under water.
            if (owner.isDiving)
            {
                owner.isGrounded = true;
            }
            if (InputManager.JumpBtnDown && owner.isInWater)
            {
                owner.StateMachine.ChangeState(PlayerJumpState.Instance);
            }
            else if (!owner.isInWater && !owner.isDiving)
            {
                owner.rigid.mass = owner.playerMass;
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
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
