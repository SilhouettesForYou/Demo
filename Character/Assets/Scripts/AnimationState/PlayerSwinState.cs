using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PlayerSwinState : MachineBehaviourState<AssistPlayerControl>
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

            // check if player is going to perform the skill under water.
            owner.IdleToPerfoemSkillUnderWater();

            if (owner.isPerformSkilUnderWater == true && Input.GetKeyDown(KeyCode.E))
            {
                owner.StateMachine.ChangeState(PerformAbilitiesState.Instance);
            }
        }

        public override void Exit()
        {
         
        }
    }
}
