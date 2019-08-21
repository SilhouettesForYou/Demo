using UnityEngine;

namespace Demo
{
    public class PlayerPushGragState : MachineBehaviourState<AssistPlayerControl>
    {
        private static PlayerPushGragState m_Instance;
        public static PlayerPushGragState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerPushGragState();
                return m_Instance;
            }
        }

        private PlayerPushGragState() { }
        public override void Enter()
        {
            //Debug.Log("Begin Drag or Push")
        }

        public override void Excute()
        {
            owner.CloseEnoughToPushable();
            if (InputManager.InteractiveBtnDown)
            {
                EventCenter.Braodcast<bool, Rigidbody2D>(EventType.Attach, true, owner.rigid);
                owner.isReleaseBox = false;
                owner.MoveWithBox();
            }
            else
            {
                EventCenter.Braodcast<bool, Rigidbody2D>(EventType.Attach, false, null);
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
        }

        public override void Exit()
        {
           
        }
    }
}
