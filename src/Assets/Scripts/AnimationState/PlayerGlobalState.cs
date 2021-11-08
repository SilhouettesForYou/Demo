using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class PlayerGlobalState : MachineBehaviourState<Pankapu>
    {
        private static PlayerGlobalState m_Instance;
        public static PlayerGlobalState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerGlobalState();
                return m_Instance;
            }
        }

        private Scene scene;

        private PlayerGlobalState() { }

        public override void Enter()
        {
        }

        public override void Excute()
        {
            //Debug.Log(owner.StateMachine.CurrentState);
            if (owner.isDead)
            {
                owner.StateMachine.ChangeState(PlayerDeadState.Instance);
            }
        }

        public override void Exit()
        {

        }

    }
}
